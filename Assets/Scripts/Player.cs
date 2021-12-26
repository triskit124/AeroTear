using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed;
    public int blinkIters;
    public float blinkTime;
    public int pitchUpAngle;
    public int pitchDownAngle;
    public enum BodyOrientation {level, pitchUp, pitchDown, rollLeft, rollRight, pitchUpRollLeft, pitchUpRollRight, pitchDownRollLeft, pitchDownRollRight};
    public BodyOrientation bodyOrientation;
    public GameObject sting;
    public GameObject shield;
    public Sprite[] jumboSprites;
    public Sprite[] vintageSprites;
    public Sprite[] shuttleSprites;
    public Sprite[] ufoSprites;

    public static float shieldTime = 10f;

    public event System.Action OnLifeLossFromCollision;
    public event System.Action OnLifeGainPickup;
    public event System.Action OnScoreBonus10Pickup;
    public event System.Action OnScoreBonus50Pickup;

    bool invincible;
    float screenHalfWidth;
    float screenHalfHeight;
    SpriteRenderer spriteRenderer;
    Sprite[] sprites;
    AudioController audioController;
    BoxCollider2D boxCollider;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioController = FindObjectOfType<AudioController>();
        boxCollider = GetComponent<BoxCollider2D>();

        OnLifeLossFromCollision += DamageBlink;

        // get player width and screen width ~~hacky, adds player width to canvas size~~
        float halfPlayerWidth = GetComponent<BoxCollider2D>().size.x * transform.localScale.x / 2;
        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize - halfPlayerWidth;

        float halfPlayerHeight = GetComponent<BoxCollider2D>().size.y * transform.localScale.y / 2;
        screenHalfHeight = Camera.main.orthographicSize - halfPlayerHeight;

        // choose sprites and player size based on which unlockable model is selected
        int modelSelected = PlayerPrefs.GetInt("modelSelected");
        switch (modelSelected) {
            case 0:
                sprites = jumboSprites;
                sting.transform.parent = null;
                transform.localScale = Vector3.one * 0.3f;
                sting.transform.parent = transform;
                boxCollider.size = new Vector2(8.3f, 3.6f);
                break;
            case 1:
                sprites = vintageSprites; 
                sting.transform.parent = null;
                transform.localScale = Vector3.one * 0.25f;
                sting.transform.parent = transform;
                boxCollider.size = new Vector2(8.3f, 3.6f);
                break;
            case 2:
                sprites = shuttleSprites; 
                sting.transform.parent = null;
                transform.localScale = Vector3.one * 0.3f;
                sting.transform.parent = transform;
                boxCollider.size = new Vector2(7.5f, 3.0f);
                break;
            case 3:
                sprites = ufoSprites;
                transform.localScale = Vector3.one * 0.2f;
                sting.transform.parent = null;
                sting.transform.parent = transform;
                sting.transform.localPosition = new Vector3(sting.transform.localPosition.x, -35f, sting.transform.localPosition.z);
                boxCollider.offset = new Vector2(0.0f, -1.0f);
                boxCollider.size = new Vector2(5.5f, 7.0f);
                break;
            default:
                sprites = jumboSprites; 
                sting.transform.parent = null;
                transform.localScale = Vector3.one * 0.3f;
                sting.transform.parent = transform;
                boxCollider.size = new Vector2(8.3f, 3.6f);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.paused && !GameManager.gameOver) {
            // handle keyboard inputs and move
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            Vector2 velocity = new Vector2(inputX * moveSpeed, inputY * moveSpeed);
            transform.Translate(velocity * Time.deltaTime, Space.World);

            // Impose movement bounds
            if (transform.position.x < -screenHalfWidth) {
                transform.position = new Vector3(-screenHalfWidth, transform.position.y, transform.position.z);
            }

            if (transform.position.x > screenHalfWidth) {
                transform.position = new Vector3(screenHalfWidth, transform.position.y, transform.position.z);
            }

            if (transform.position.y < -screenHalfHeight) {
                transform.position = new Vector3(transform.position.x, -screenHalfHeight, transform.position.z);
            }

            if (transform.position.y > screenHalfHeight) {
                transform.position = new Vector3(transform.position.x, screenHalfHeight, transform.position.z);
            }

            // set body orientation
            bool A = Input.GetKey(KeyCode.A);
            bool W = Input.GetKey(KeyCode.W);
            bool S = Input.GetKey(KeyCode.S); 
            bool D = Input.GetKey(KeyCode.D);

            if (A && !W && !S && !D) {
                bodyOrientation = BodyOrientation.pitchUp;
                spriteRenderer.sprite = sprites[0];
                sting.transform.parent = null;
                transform.rotation = Quaternion.Euler(0, 0, pitchUpAngle);
                sting.transform.parent = transform;
            } else if (!A && !W && !S && D) {
                bodyOrientation = BodyOrientation.pitchDown;
                spriteRenderer.sprite = sprites[0];
                sting.transform.parent = null;
                transform.rotation = Quaternion.Euler(0, 0, pitchDownAngle);
                sting.transform.parent = transform;
            } else if (!A && W && !S && !D) {
                bodyOrientation = BodyOrientation.rollLeft;
                spriteRenderer.sprite = sprites[2];
                sting.transform.parent = null;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                sting.transform.parent = transform;
            } else if (!A && !W && S && !D) {
                bodyOrientation = BodyOrientation.rollRight;
                spriteRenderer.sprite = sprites[1];
                sting.transform.parent = null;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                sting.transform.parent = transform;
            } else if (A && W && !S && !D) {
                bodyOrientation = BodyOrientation.pitchUpRollLeft;
                spriteRenderer.sprite = sprites[2];
                sting.transform.parent = null;
                transform.rotation = Quaternion.Euler(0, 0, pitchUpAngle);
                sting.transform.parent = transform;
            } else if (A && !W && S && !D) {
                bodyOrientation = BodyOrientation.pitchUpRollRight;
                spriteRenderer.sprite = sprites[1];
                sting.transform.parent = null;
                transform.rotation = Quaternion.Euler(0, 0, pitchUpAngle);
                sting.transform.parent = transform;
            } else if (!A && W && !S && D) {
                bodyOrientation = BodyOrientation.pitchDownRollLeft;
                spriteRenderer.sprite = sprites[2];
                sting.transform.parent = null;
                transform.rotation = Quaternion.Euler(0, 0, pitchDownAngle);
                sting.transform.parent = transform;
            } else if (!A && !W && S && D) {
                bodyOrientation = BodyOrientation.pitchDownRollRight;
                spriteRenderer.sprite = sprites[1];
                sting.transform.parent = null;
                transform.rotation = Quaternion.Euler(0, 0, pitchDownAngle);
                sting.transform.parent = transform;
            } else {
                bodyOrientation = BodyOrientation.level;
                spriteRenderer.sprite = sprites[0];
                sting.transform.parent = null;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                sting.transform.parent = transform;
            }
            //print(bodyOrientation);
        }

    }

    // handle a collision betwixt player and a block
    void OnTriggerEnter2D(Collider2D triggerCollider) {
        if(triggerCollider.tag == "Obstacle") {
            if (!invincible) {
                OnLifeLossFromCollision();
                Destroy(triggerCollider.gameObject);
            }
        } else if (triggerCollider.tag == "Pickup+1") {
            OnLifeGainPickup();
            Destroy(triggerCollider.gameObject);
        } else if (triggerCollider.tag == "Pickup10") {
            OnScoreBonus10Pickup();
            Destroy(triggerCollider.gameObject);
        } else if (triggerCollider.tag == "Pickup50") {
            OnScoreBonus50Pickup();
            Destroy(triggerCollider.gameObject);
        } else if (triggerCollider.tag == "PickupShield") {
            ShieldPickup();
            Destroy(triggerCollider.gameObject);
        }
    }

    void ShieldPickup() {
        audioController.Play("shield_sound");
        GameObject newShield= (GameObject)Instantiate(shield, transform.position, Quaternion.identity);
        newShield.transform.parent = transform;
        //StartCoroutine(ShieldInvincibility());
    }

    /*
    IEnumerator ShieldInvincibility() {
        //GetComponent<BoxCollider2D>().isTrigger = false;
        //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        //yield return new WaitForSeconds(shieldTime);

        //GetComponent<BoxCollider2D>().isTrigger = true;
        //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
    */

    void DamageBlink() {
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine() {
        invincible = true;
        for (int i = 0; i < blinkIters; i++) {
            if (spriteRenderer.enabled) {
                spriteRenderer.enabled = false;
            } else {
                spriteRenderer.enabled = true;
            }
            yield return new WaitForSeconds(blinkTime);
        }
        invincible = false;
    }
}
