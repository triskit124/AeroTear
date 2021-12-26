using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CautionIndicator : MonoBehaviour
{

    public int blinkIters;
    public float blinkTime;
    public float moveSpeed;

    SpriteRenderer sprite;
    GameObject trackedObject;
    
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        FindObjectOfType<AudioController>().Play("caution_sound");
        CautionBlink();
    }

    // Update is called once per frame
    void Update()
    {
        float dir = trackedObject.transform.position.y - transform.position.y;
        transform.Translate(Vector3.up * dir * moveSpeed * Time.deltaTime);

        if (trackedObject.transform.position.x <= 8.2) {
            Destroy(gameObject);
        }
    }

    void CautionBlink() {
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine() {
        for (int i = 0; i < blinkIters; i++) {
            if (sprite.enabled) {
                sprite.enabled = false;
            } else {
                sprite.enabled = true;
            }
            yield return new WaitForSeconds(blinkTime);
        }
    }

    public void SetTrackingObject(GameObject obj) {
        trackedObject = obj;
    }
}
