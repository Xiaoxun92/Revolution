using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : MonoBehaviour
{
    [Header("完全冷却时的颜色")]
    public Color colorCold;
    [Header("点燃前的颜色")]
    public Color colorHot;
    [Header("点燃需要的时间")]
    public float igniteTime;

    GameManager gameManager;

    public float growTime;
    public float attractMaxSpeed;
    public float attractAcceleration;

    SpriteRenderer sr;
    Transform player;
    Player playerScript;
    Color colorDelta;
    public bool burning;
    float moveSpeed;

    int timer;
    Vector3 direction;

    void Start()
    {
        gameManager = Camera.main.GetComponent<GameManager>();

        sr = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<Player>();
        colorDelta = (colorHot - colorCold) / igniteTime;
        burning = false;
        moveSpeed = 0;

        timer = 1;
        direction = new Vector2();
    }

    void FixedUpdate()
    {
        if (burning == false) {

            float timeMin = 0;
            float timeMax = 0;
            float speed = 0;

            if (gameManager.mouseControlMode) {
                timeMin = 2;
                timeMin = 4;
                speed = 0.015f;
            } else {
                timeMin = 2;
                timeMin = 4;
                speed = 0.005f;
            }

            timer--;
            if (timer == 0) {
                timer = (int)Random.Range(timeMin, timeMax) * 60;
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
                sr.color += colorDelta * Time.fixedDeltaTime * playerScript.fireSize;
                if (sr.color.r >= colorHot.r) {
                    burning = true;
                    sr.color = colorHot;
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            } else {
                sr.color -= colorDelta * Time.fixedDeltaTime;
                if (sr.color.r < colorCold.r)
                    sr.color = colorCold;
            }
            return;
        }

        // Burning
        if (growTime > 0) {
            growTime -= Time.fixedDeltaTime;
            return;
        }
        if (moveSpeed < attractMaxSpeed)
            moveSpeed += attractAcceleration;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed);
        if (Vector2.Distance(transform.position, player.position) < 0.1 * playerScript.fireSize) {
            playerScript.Grow();
            Destroy(gameObject);
        }
    }
}
