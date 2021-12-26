using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // needed to switch scenes

public class GameManager : MonoBehaviour
{

    public Text scoreDisplay;
    public Text livesDisplay;
    public Text bestScoreDisplay;
    public Text newBestScoreDisplay;
    public Text lifetimeScoreDisplay;
    public Text windSpeedDisplay;
    public GameObject pausedUIHolder;
    public GameObject pauseButton;
    public GameObject gameOverGUIHolder;

    static float score;
    public static int lives;
    public static float difficultyMultiplier;
    public static bool gameOver;
    public static bool paused;
    static float timeAlive;

    AudioController audioController;
    string sceneName;
    bool lifeWasLost;
    float scoreWhenlifeWasLost;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Main") {
            // reset static variables
            UnPause();
            score = 0;
            lives = 5;
            livesDisplay.text = lives.ToString();
            bestScoreDisplay.text = PlayerPrefs.GetInt("bestSingleGameScore").ToString();
            difficultyMultiplier = 1;
            gameOver = false;
            timeAlive = 0;

            FindObjectOfType<Player>().OnLifeLossFromCollision += LifeLossFromCollision;
            FindObjectOfType<Player>().OnLifeGainPickup += LifeGainPickup;
            FindObjectOfType<Player>().OnScoreBonus10Pickup+= ScoreBonus10Pickup;
            FindObjectOfType<Player>().OnScoreBonus50Pickup+= ScoreBonus50Pickup;
            FindObjectOfType<RequestedSetupSpawner>().OnLifeLossFromFailedRequest += LifeLossFromFailedRequest;
        }

        audioController = FindObjectOfType<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName == "Main") {
                timeAlive += Time.deltaTime;
                difficultyMultiplier = DifficultyFunction(timeAlive);
                //print(difficultyMultiplier);
                score += Time.deltaTime;
                scoreDisplay.text = Mathf.Round(score).ToString();
                windSpeedDisplay.text = Mathf.Round(Mathf.Lerp(0, 500, difficultyMultiplier / 10f)).ToString() + " mph";
                windSpeedDisplay.color = new Color(Mathf.Lerp(0f, 1f, difficultyMultiplier / 10f), 0f, 0f, 1f);

                if (score > PlayerPrefs.GetInt("bestSingleGameScore")) {
                    bestScoreDisplay.text = Mathf.Round(score).ToString();
                }

            if (Input.GetKeyDown(KeyCode.Space)) {
                if (gameOver) {
                    Restart();
                } else if (paused) {
                    UnPause();
                } else {
                    Pause();
                }
            }
        }

    }

    float DifficultyFunction(float t) {
        // sigmoid function
        float L = 10; // max difficult value
        float k = 0.025f; // steepness of growthrate
        float t0 = 60; // sigmoid midpoint
        return L / (1 + Mathf.Exp(-k * (t - t0)));
    }

    void LifeLossFromCollision() {
        lives --;
        if (!lifeWasLost) {
            scoreWhenlifeWasLost = score;
            lifeWasLost = true;
        }
        audioController.Play("damage_sound");
        livesDisplay.text = lives.ToString();
        if (lives <= 0 ) {
            GameOver();
        }
    }

    void LifeLossFromFailedRequest() {
        lives --;
        if (!lifeWasLost) {
            scoreWhenlifeWasLost = score;
            lifeWasLost = true;
        }
        livesDisplay.text = lives.ToString();
        if (lives <= 0 ) {
            GameOver();
        }
    }

    void LifeGainPickup() {
        audioController.Play("coin_sound");
        lives++;
        livesDisplay.text = lives.ToString();
    }

    void ScoreBonus10Pickup() {
        audioController.Play("coin_sound");
        score+=10;
    }

    void ScoreBonus50Pickup() {
        audioController.Play("coin_sound");
        score+=50;
    }

    void GameOver() {
        audioController.Play("game_over_sound");
        gameOver = true;
        Time.timeScale = 0;
        gameOverGUIHolder.SetActive(true);
        pauseButton.GetComponent<Image>().enabled = false;


        PlayerPrefs.SetInt("lifetimeScore", PlayerPrefs.GetInt("lifetimeScore") + (int)Mathf.Round(score));
        lifetimeScoreDisplay.text = "Lifetime Score: " + PlayerPrefs.GetInt("lifetimeScore").ToString();

        if (score > PlayerPrefs.GetInt("bestSingleGameScore")) {
            print("new best score");
            PlayerPrefs.SetInt("bestSingleGameScore", (int)Mathf.Round(score));
            newBestScoreDisplay.text = "New Best Score: " +  PlayerPrefs.GetInt("bestSingleGameScore").ToString();
        }

        if (scoreWhenlifeWasLost > PlayerPrefs.GetInt("bestNoLifeLossScore")) {
            print("new best flawless score");
            PlayerPrefs.SetInt("bestNoLifeLossScore", (int)Mathf.Round(scoreWhenlifeWasLost));
        }
    }

    public void Pause() {
        Time.timeScale = 0;
        paused = true;
        pausedUIHolder.SetActive(true);
        pauseButton.GetComponent<Image>().enabled = false;
    }

    public void UnPause() {
        Time.timeScale = 1;
        paused = false;
        pausedUIHolder.SetActive(false);
        pauseButton.GetComponent<Image>().enabled = true;
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload the level
    }

    public void Play() {
        SceneManager.LoadScene("Main");
    }

    public void GoToMainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void GoToUnlocks() {
        SceneManager.LoadScene("Unlocks");
    }

    public void GoToAbout() {
        SceneManager.LoadScene("About");
    }
    
}
