using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public float speedMin;
    public float speedMax;
    public float rotSpeedMin;
    public float rotSpeedMax;
    public float dirStd;

    float speed;
    float rotSpeed;
    float dir;
    new Rigidbody2D rigidbody2D;

    void Awake()
    {
        speed = Random.Range(speedMin * GameManager.difficultyMultiplier, speedMax * GameManager.difficultyMultiplier);
        dir = Util.SampleFromNormalDist(0f, dirStd); // normal dist
        rotSpeed = Random.Range(rotSpeedMin, rotSpeedMax);
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody2D.AddForce(Quaternion.Euler(0, 0, dir) * Vector3.left * speed);
        transform.Rotate(new Vector3(0f, 0f, 1f) * rotSpeed * Time.fixedDeltaTime);

        // get rid of block after its left the screen
        if (transform.position.x < -15) {
            Destroy(gameObject);
        }
    }
}
