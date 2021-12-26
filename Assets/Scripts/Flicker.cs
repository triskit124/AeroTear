using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{

    //WaitForSeconds shortWait = new WaitForSeconds(0.1f);
    //WaitForSeconds longWait = new WaitForSeconds(5.0f);

    public int difficultyThreshold;
    public int maxFlickers;
    public float flickerWaitTimeMin;
    public float flickerWaitTimeMax;
    public float flickerTimeOnMin;
    public float flickerTimeOnMax;
    public float flickerTimeOffMin;
    public float flickerTimeOffMax;
    public float flickerDimFactor;

    float waitTime;
    bool on;
    new Light light;


    // Start is called before the first frame update
    void Start()
    {
        waitTime = Random.Range(flickerWaitTimeMin, flickerWaitTimeMax);
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (GameManager.difficultyMultiplier > difficultyThreshold) {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0) {
                StartCoroutine(FlickerRoutine());
                waitTime = Random.Range(flickerWaitTimeMin, flickerWaitTimeMax);
            }
        }
    }

    IEnumerator FlickerRoutine() {
        int numFlickers = Random.Range(1, maxFlickers);
        for (int i = 0; i < numFlickers; i++) {
            light.intensity -= flickerDimFactor;
            yield return new WaitForSeconds(Random.Range(flickerTimeOffMin, flickerTimeOffMax));
            light.intensity += flickerDimFactor;
            yield return new WaitForSeconds(Random.Range(flickerTimeOnMin, flickerTimeOnMax));
        }
    }

}
