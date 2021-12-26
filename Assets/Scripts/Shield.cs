using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    public float fadeInTime;
    public float fadeOutTime;
    public float blinkTime;
    public int numBlinks;

    SpriteRenderer spriteRenderer;
    Color c;
    float timeAlive;
    bool fadedIn;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ShieldRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (fadedIn) {
            timeAlive += Time.deltaTime;

            c = Color.Lerp(Color.white, Color.yellow, Mathf.Sin(3 * Time.time)); 
            c.a = Mathf.Lerp(1, 0, timeAlive / Player.shieldTime); 
            spriteRenderer.color = c;
        }
    }

    IEnumerator ShieldRoutine() {
        // fade in
        for (int i = 0; i < 10; i++) {
            c = spriteRenderer.color;
            c.a = Mathf.Lerp(0, 1, i / 10f);
            spriteRenderer.color = c;
            yield return new WaitForSeconds(fadeInTime / 10);
        }
        fadedIn = true;

        yield return new WaitForSeconds(Player.shieldTime - 1);
        Destroy(gameObject);
    }
}
