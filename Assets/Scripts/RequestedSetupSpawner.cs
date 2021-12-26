using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestedSetupSpawner : MonoBehaviour
{

    public float spawnTimeMin;
    public float spawnTimeMax;
    public float waitTimeMin;
    public float waitTimeMax;
    public Text requestedSetupDisplay;
    public Text countdownTicker;
    public GameObject[] frames;
    public GameObject[] speechBubblePrefabs; 

    public event System.Action OnLifeLossFromFailedRequest;

    bool waitingForTakeData;
    float spawnTime;
    Player.BodyOrientation requestedBodyOrientation;
    int idx;
    int prevIdx;
    AudioController audioController;
    GameObject frame;

    // Start is called before the first frame update
    void Start()
    {
        audioController = FindObjectOfType<AudioController>();
        spawnTime = ChooseNewSpawnTime();
        
        // choose sprites and player size based on which unlockable model is selected
        frame = frames[PlayerPrefs.GetInt("modelSelected")];
        frame.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitingForTakeData) {
            spawnTime -= Time.deltaTime;
            if (spawnTime <= 0) {
                // pick out a random orientation
                idx = Random.Range(1, System.Enum.GetValues(typeof(Player.BodyOrientation)).Length);
                requestedBodyOrientation = (Player.BodyOrientation)idx;

                requestedSetupDisplay.text = requestedBodyOrientation.ToString();

                frame.transform.GetChild(idx).gameObject.SetActive(true);
                frame.transform.GetChild(0).gameObject.SetActive(false);

                GameObject speechBubbleToSpawn = speechBubblePrefabs[Random.Range(0, speechBubblePrefabs.Length)];
                Vector3 speechBubblePosition = new Vector3(5, 3.7f, 0);
                GameObject newSpeechBubble = (GameObject)Instantiate(speechBubbleToSpawn, speechBubblePosition, Quaternion.identity);
                newSpeechBubble.transform.parent = transform.parent;

                waitingForTakeData = true;
                float waitTime = Random.Range(waitTimeMin, waitTimeMax); // modulate based on diff?
                //print("new request is " + requestedBodyOrientation.ToString());
                audioController.Play("new_request_sound");
                StartCoroutine(WaitForTakeDataCountdown(waitTime));
            }
        }
        
    }

    float ChooseNewSpawnTime() {
        float difficultyShift = 2/10 * GameManager.difficultyMultiplier; // maxes out at 2
        return Random.Range(spawnTimeMin - difficultyShift, spawnTimeMax - difficultyShift);
    }

    IEnumerator WaitForTakeDataCountdown(float waitTime) {
            //print("waiting " + waitTime + " seconds");
            yield return new WaitForSeconds(waitTime);
            yield return StartCoroutine(StartCountdown(3, 1));
            CountdownFinished();
    }

    IEnumerator StartCountdown(int startingCount, float countdownWaitTime) {
        countdownTicker.enabled = true;
        for (int i = startingCount; i > 0; i--) {
            audioController.Play("countdown_sound");
            countdownTicker.text = i.ToString();
            yield return new WaitForSeconds(countdownWaitTime);
        }
    }

    void CountdownFinished() {
        if (FindObjectOfType<Player>().GetComponent<Player>().bodyOrientation == requestedBodyOrientation) {
            audioController.Play("request_pass_sound");
        } else {
            OnLifeLossFromFailedRequest();
            audioController.Play("request_fail_sound");
        }
        waitingForTakeData = false;
        countdownTicker.enabled = false;
        spawnTime = ChooseNewSpawnTime();
        frame.transform.GetChild(0).gameObject.SetActive(true);
        frame.transform.GetChild(idx).gameObject.SetActive(false);
    }
}
