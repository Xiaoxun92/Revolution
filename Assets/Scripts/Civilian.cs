using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : MonoBehaviour
{
    [Header("点燃需要的时间")]
    public float igniteTime;

    GameManager gameManager;

    public float growTime;
    public float roamSpeed;
    public float attractMaxSpeed;
    public float attractAcceleration;

    ParticleSystem particle;
    Transform player;
    Player playerScript;
    
    public bool burning;
    float ignitePercentage;
    
    float moveSpeed;

    int timer;
    Vector3 direction;

    void Start()
    {
        gameManager = Camera.main.GetComponent<GameManager>();

        particle = gameObject.GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<Player>();

        burning = false;
        ignitePercentage = 0;

        moveSpeed = 0;

        timer = 1;
        direction = new Vector2();
    }

    void FixedUpdate()
    {
        if (gameManager.stateChanging)
            return;

        if (burning == false) {

            float timeMin = 0;
            float timeMax = 0;
            float speed = 0;

            if (gameManager.mouseControlMode) {
                timeMin = 2;
                timeMin = 4;
                speed = roamSpeed;
            } else {
                timeMin = 2;
                timeMin = 4;
                speed = 0.005f;
            }

            timer--;
            if (timer <= 0) {
                timer = (int)(Random.Range(timeMin, timeMax) * 60);
                int d = Random.Range(0, 3);
                switch (d) {
                    case 0:
                        direction = new Vector2(1, 0);
                        break;
                    case 1:
                        direction = new Vector2(0, 1);
                        break;
                    case 2:
                        direction = new Vector2(-1, 0);
                        break;
                    case 3:
                        direction = new Vector2(0, -1);
                        break;
                }
            }
            transform.position += direction * speed;
            
            if (playerScript.burning && Vector2.Distance(transform.position, player.position) < playerScript.igniteRadius) {
                ignitePercentage += Time.deltaTime / igniteTime;
                transform.GetChild(0).GetChild(0).GetComponent<CircleZoom>().enabled = false;
                transform.GetChild(0).GetChild(2).GetComponent<CircleZoom>().enabled = false;
                if (ignitePercentage >= 1) {
                    burning = true;
                    transform.GetChild(1).gameObject.SetActive(true);
                }
            } else {
                if (ignitePercentage > 0)
                    ignitePercentage -= Time.deltaTime / igniteTime;
                transform.GetChild(0).GetChild(0).GetComponent<CircleZoom>().enabled = true;
                transform.GetChild(0).GetChild(2).GetComponent<CircleZoom>().enabled = true;
            }
            SetAlpha(transform.GetChild(0).GetChild(0), 1 - ignitePercentage);
            SetAlpha(transform.GetChild(0).GetChild(1), ignitePercentage);
            SetAlpha(transform.GetChild(0).GetChild(2), 1 - ignitePercentage);
            return;
        }

        // Burning
        if (growTime > 0) {
            growTime -= Time.fixedDeltaTime;
            return;
        }
        if (transform.GetChild(0).localScale.x > 0) {
            transform.GetChild(0).localScale -= Vector3.one * 0.02f;
        }
        if (moveSpeed < attractMaxSpeed)
            moveSpeed += attractAcceleration;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed);
        if (Vector2.Distance(transform.position, player.position) < 0.1) {
            playerScript.Grow();
            playerScript.lockSize = false;
            Destroy(gameObject);
        }
    }

    void SetAlpha(Transform child, float alpha)
    {
        Color c = child.GetComponent<SpriteRenderer>().color;
        c.a = alpha;
        child.GetComponent<SpriteRenderer>().color = c;
    }
}
