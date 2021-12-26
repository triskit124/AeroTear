using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupsSpawner : MonoBehaviour
{

    public float spawnTimeMin;
    public float spawnTimeMax;
    public GameObject[] pickupPrefabs;
    public GameObject cautionIndicatorPrefab;

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
            GameObject pickupToSpawn = pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];
            Vector3 spawnPosition = new Vector3(screenHalfSize.x + 5, Random.Range(-screenHalfSize.y /2, screenHalfSize.y / 2));
            GameObject newPickup = (GameObject)Instantiate(pickupToSpawn, spawnPosition, Quaternion.identity);
            newPickup.transform.parent = transform;

            GameObject newCautionIndicator = (GameObject)Instantiate(cautionIndicatorPrefab, new Vector3(8.2f, spawnPosition.y, 0), Quaternion.identity);
            newCautionIndicator.GetComponent<CautionIndicator>().SetTrackingObject(newPickup);
            newCautionIndicator.transform.parent = transform;

            spawnTime = ChooseNewSpawnTime();
        }
    }

    float ChooseNewSpawnTime() {
        return Random.Range(spawnTimeMin, spawnTimeMax);
    }
}
