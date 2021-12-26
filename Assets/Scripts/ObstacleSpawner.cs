using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    public float spawnTimeMin;
    public float spawnTimeMax;
    public GameObject[] obstaclePrefabs;
    public GameObject cautionIndicatorPrefab;
    //public AnimationCurve positionProbabilityCurve;

    float spawnTime;
    Vector2 screenHalfSize;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = ChooseNewSpawnTime();
        screenHalfSize = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime -= Time.deltaTime;

        if (spawnTime <= 0f) {
            // spawn a new obsticle
            GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Vector3 spawnPosition = new Vector3(screenHalfSize.x + 5, Random.Range(-screenHalfSize.y /2, screenHalfSize.y / 2));
            GameObject newObstacle = (GameObject)Instantiate(obstacleToSpawn, spawnPosition, Quaternion.identity);
            newObstacle.transform.parent = transform;

            GameObject newCautionIndicator = (GameObject)Instantiate(cautionIndicatorPrefab, new Vector3(8.2f, spawnPosition.y, 0), Quaternion.identity);
            newCautionIndicator.GetComponent<CautionIndicator>().SetTrackingObject(newObstacle);
            newCautionIndicator.transform.parent = transform;

            spawnTime = ChooseNewSpawnTime();
        }
    }

    float ChooseNewSpawnTime() {
        return Random.Range(spawnTimeMin / GameManager.difficultyMultiplier, spawnTimeMax / GameManager.difficultyMultiplier);
    }
}
