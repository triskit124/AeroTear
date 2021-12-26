using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jitter : MonoBehaviour
{

    public float jitterX;
    public float jitterY;
    public float jitterTime;
    public float difficultyThreshold;

    Vector3 startPos;
    float jitterTimer;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        jitterTimer = jitterTime;
    }

    void Update () {
        if (GameManager.difficultyMultiplier > difficultyThreshold) {
            jitterTimer -= Time.deltaTime;
            if (jitterTimer <= 0) {
                float rangeX = Mathf.Lerp(0, jitterX, (GameManager.difficultyMultiplier - difficultyThreshold) / (10f - difficultyThreshold));
                float rangeY = Mathf.Lerp(0, jitterY, (GameManager.difficultyMultiplier - difficultyThreshold) / (10f - difficultyThreshold));
                Vector3 newPos = new Vector3(startPos.x + Random.Range(-rangeX, rangeX), startPos.y + Random.Range(-rangeY, rangeY), startPos.z);
                transform.position = newPos;
                jitterTimer = jitterTime;
            }
        }
    }
}
