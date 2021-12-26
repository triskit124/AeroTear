using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{

    public int wiggleIters;
    public float wiggleTime;
    public float wiggleAngle;
    public float loiterTime;
    public float fadeInTime;
    public float fadeOutTime;

    SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(WiggleRoutine());
    }

    IEnumerator WiggleRoutine() {
        // fade in
        for (int i = 0; i < 10; i++) {
            Color c = spriteRenderer.color;
            c.a = Mathf.Lerp(0, 1, i / 10f);
            spriteRenderer.color = c;
            yield return new WaitForSeconds(fadeInTime / 10);
        }

        // wiggle it
        for (int i = 0; i < wiggleIters; i++) {
            transform.rotation = Quaternion.Euler(0, 0, -wiggleAngle);
            yield return new WaitForSeconds(wiggleTime);
            transform.rotation = Quaternion.Euler(0, 0, wiggleAngle);
            yield return new WaitForSeconds(wiggleTime);
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(loiterTime);

        // fade out
        for (int i = 0; i < 10; i++) {
            Color c = spriteRenderer.color;
            c.a = Mathf.Lerp(1, 0, i / 10f);
            spriteRenderer.color = c;
            yield return new WaitForSeconds(fadeOutTime / 10);
        }
        Destroy(gameObject);
    }
}
