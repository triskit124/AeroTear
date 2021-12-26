using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnlockablesManager : MonoBehaviour
{

    public GameObject unlockablesHolder;
    public Button leftArrow;
    public Button rightArrow;
    public Text descriptionText;
    public Text descriptionSubText;
    public Text bestScoreDisplay;
    public Text lifetimeScoreDisplay;
    public Text bestFlawlessScoreDisplay;
    public GameObject jumboPrefab;
    public GameObject vintagePrefab;
    public GameObject shuttlePrefab;
    public GameObject ufoPrefab;
    public Sprite selectSprite;
    public Sprite selectedSprite;
    public Button selectionButton;

    public float moveDist;
    public float moveTime;
    public float moveFPS;

    int jumboUnlocked;
    int vintageUnlocked;
    int shuttleUnlocked;
    int ufoUnlocked;
    int selection;
    bool moving;

    int lifetimeScoreToUnlockVintage = 1000;
    int bestScoreToUnlockShuttle = 250;
    public static int bestNoLifeLossScoreToUnlockUfo = 120;

    // Start is called before the first frame update
    void Start()
    {

        // WARNING these override the unlock system for debugging
        //PlayerPrefs.SetInt("lifetimeScore", 0);
        //PlayerPrefs.SetInt("bestSingleGameScore", 0);
        //PlayerPrefs.SetInt("bestNoLifeLossScore", 0);
        //PlayerPrefs.SetInt("vintageUnlocked", 0);
        //PlayerPrefs.SetInt("shuttleUnlocked", 0);
        //PlayerPrefs.SetInt("ufoUnlocked", 0);

        HandleUnlocks();
        UpdateSelectionInfo();
        
    }

    public void SelectCurrentModel() {
        PlayerPrefs.SetInt("modelSelected", selection);
        UpdateSelectionInfo();
    }

    public void nextModel() {
        if (!moving) {
            if (selection < 3) {
                StartCoroutine(AnimatedMove(unlockablesHolder.transform.position.x, unlockablesHolder.transform.position.x - moveDist, moveTime));
                selection++;
                UpdateSelectionInfo();
            } 

            if (selection > 0) {
                leftArrow.GetComponent<Image>().enabled = true;
            }

            if (selection == 3) {
                rightArrow.GetComponent<Image>().enabled = false;
            } 
        }
    }

    public void prevModel() {
        if (!moving) {
            if (selection > 0) {
                StartCoroutine(AnimatedMove(unlockablesHolder.transform.position.x, unlockablesHolder.transform.position.x + moveDist, moveTime));
                selection--;
                UpdateSelectionInfo();
            }

            if (selection < 3) {
                rightArrow.GetComponent<Image>().enabled = true;
            } 

            if (selection == 0) {
                leftArrow.GetComponent<Image>().enabled = false;
            }
        }
    }

    IEnumerator AnimatedMove(float from, float to, float moveTime) {
        float numFrames = moveFPS * moveTime;
        //float numFrames = moveTime / Time.deltaTime; // calculates how many frames are needed based on current framerate (ie 1 / deltaTime)
        moving = true;

        for (int i = 0; i < numFrames; i++) {
            unlockablesHolder.transform.position = new Vector3(Mathf.SmoothStep(from, to, i / (numFrames - 1)), unlockablesHolder.transform.position.y, 0);
            yield return new WaitForSeconds(moveTime / numFrames);
        }
        moving = false;
    }

    void UpdateSelectionInfo() {
        int modelSelected = PlayerPrefs.GetInt("modelSelected");
        if (selection == 0 && jumboUnlocked == 1) {
            descriptionText.text = "Jumbo.";
            descriptionSubText.text = "Just a big boi";
            if (modelSelected == selection) {
                selectionButton.image.sprite = selectedSprite;
                selectionButton.interactable = false;
            } else {
                selectionButton.image.sprite = selectSprite;
                selectionButton.interactable = true;
            }
        } else if (selection == 1 && vintageUnlocked == 1) {
            descriptionText.text = "Vintage.";
            descriptionSubText.text = "Past its\ndesign life";
            if (modelSelected == selection) {
                selectionButton.image.sprite = selectedSprite;
                selectionButton.interactable = false;
            } else {
                selectionButton.image.sprite = selectSprite;
                selectionButton.interactable = true;
            }
        } else if (selection == 1 && vintageUnlocked == 0) {
            descriptionText.text = "???";
            descriptionSubText.text = "Get to " + lifetimeScoreToUnlockVintage.ToString()+"\nlifetime score\nto unlock";
            selectionButton.interactable = false;
            selectionButton.image.sprite = selectSprite;
        } else if (selection == 2 && shuttleUnlocked == 1) {
            descriptionText.text = "Shuttle.";
            descriptionSubText.text = "Is this tunnel\nfast enough?";
            if (modelSelected == selection) {
                selectionButton.image.sprite = selectedSprite;
                selectionButton.interactable = false;
            } else {
                selectionButton.image.sprite = selectSprite;
                selectionButton.interactable = true;
            }
        } else if (selection == 2 && shuttleUnlocked == 0) {
            descriptionText.text = "???";
            descriptionSubText.text = "Earn a score of\n" + bestScoreToUnlockShuttle.ToString() + " in one game\nto unlock";
            selectionButton.interactable = false;
            selectionButton.image.sprite = selectSprite; 
        } else if (selection == 3 && ufoUnlocked == 1) {
            descriptionText.text = "UFO.";
            descriptionSubText.text = "They seem\nfriendly";
            if (modelSelected == selection) {
                selectionButton.image.sprite = selectedSprite;
                selectionButton.interactable = false;
            } else {
                selectionButton.image.sprite = selectSprite;
                selectionButton.interactable = true;
            }
        } else if (selection == 3 && ufoUnlocked == 0) {
            descriptionText.text = "???";
            descriptionSubText.text = "Earn a score of\n" + bestNoLifeLossScoreToUnlockUfo.ToString() + " without losing\na life to unlock";
            selectionButton.interactable = false;
            selectionButton.image.sprite = selectSprite;
        }
    }

    void HandleUnlocks() {

        bestScoreDisplay.text = "Best Score: " + PlayerPrefs.GetInt("bestSingleGameScore").ToString();
        lifetimeScoreDisplay.text = "Lifetime Score: " + PlayerPrefs.GetInt("lifetimeScore").ToString();
        bestFlawlessScoreDisplay.text = "Best No Life Loss Score: " + PlayerPrefs.GetInt("bestNoLifeLossScore").ToString();

        jumboUnlocked = PlayerPrefs.GetInt("jumboUnlocked");
        vintageUnlocked = PlayerPrefs.GetInt("vintageUnlocked");
        shuttleUnlocked = PlayerPrefs.GetInt("shuttleUnlocked");
        ufoUnlocked = PlayerPrefs.GetInt("ufoUnlocked");

        //print("lifetimescore = " + PlayerPrefs.GetInt("lifetimeScore"));
        //print("bestscore = " + PlayerPrefs.GetInt("bestSingleGameScore"));
        //print("bestnolifelossscore= " + PlayerPrefs.GetInt("bestNoLifeLossScore"));

        if (PlayerPrefs.GetInt("lifetimeScore") >= lifetimeScoreToUnlockVintage) {
            PlayerPrefs.SetInt("vintageUnlocked", 1);
            vintageUnlocked = 1;
        } else {
            PlayerPrefs.SetInt("vintageUnlocked", 0);
            vintageUnlocked = 0;
        }

        if (PlayerPrefs.GetInt("bestSingleGameScore") >= bestScoreToUnlockShuttle) {
            PlayerPrefs.SetInt("shuttleUnlocked", 1);
            shuttleUnlocked = 1;
        } else {
            PlayerPrefs.SetInt("shuttleUnlocked", 0);
            shuttleUnlocked = 0;
        }

        if (PlayerPrefs.GetInt("bestNoLifeLossScore") >= bestNoLifeLossScoreToUnlockUfo) {
            PlayerPrefs.SetInt("ufoUnlocked", 1);
            ufoUnlocked = 1;
        } else {
            PlayerPrefs.SetInt("ufoUnlocked", 0);
            ufoUnlocked = 0;
        }

        // Enable/disable sprites as needed
        if (jumboUnlocked == 1) {
            jumboPrefab.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            PlayerPrefs.SetInt("jumboUnlocked", 1);
            jumboUnlocked = 1;
            jumboPrefab.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (vintageUnlocked == 1) {
            vintagePrefab.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            vintagePrefab.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (shuttleUnlocked == 1) {
            shuttlePrefab.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            shuttlePrefab.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (ufoUnlocked == 1) {
            ufoPrefab.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            ufoPrefab.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
