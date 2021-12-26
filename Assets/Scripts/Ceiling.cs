using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceiling : MonoBehaviour
{

    public GameObject crumbPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Vector3 spawnPosition = collision.contacts[0].point;
        spawnPosition.y = 4.7f;
        spawnPosition.z = 0;
        GameObject newCrumb = (GameObject)Instantiate(crumbPrefab, spawnPosition, Quaternion.identity);
    }
}
