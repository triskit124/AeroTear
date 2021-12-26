using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crumb : MonoBehaviour
{

    public float lifetime;

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) {
            Destroy(transform.gameObject);
        }
    }
}
