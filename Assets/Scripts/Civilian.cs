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

    ParticleSystem particle;
    Transform player;
    Player playerScript;
    Color colorDelta;
    Color currentColor;
    float sizeX, sizeY;
    public bool burning;
    float moveSpeed;

    int timer;
    Vector3 direction;

    void Start()
    {
        gameManager = Camera.main.GetComponent<GameManager>();

        particle = gameObject.GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<Player>();
        colorDelta = (colorHot - colorCold) / igniteTime;
        currentColor = colorCold;
        sizeX = 2.8f;
        sizeY = 3f;
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

            var main = particle.main;
            Color c = currentColor;
            if (playerScript.burning && Vector2.Distance(transform.position, player.position) < playerScript.igniteRadius) {
                currentColor = c + colorDelta * Time.fixedDeltaTime * playerScript.fireSize;
                main.startColor = currentColor;
                if (currentColor.r >= colorHot.r) {
                    burning = true;
                    main.startColor = colorHot;
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(true);
                    transform.GetChild(2).gameObject.SetActive(true);
                }
            } else {
                currentColor = c - colorDelta * Time.fixedDeltaTime;
                main.startColor = currentColor;
                if (currentColor.r < colorCold.r) {
                    currentColor = colorCold;
                    main.startColor = currentColor;
                }
            }
            return;
        }

        // Burning
        var m = particle.main;
        if (sizeX > 0) {
            sizeX -= 0.1f;
            m.startSizeX = sizeX;
        }
        if (sizeY > 0) {
            sizeY -= 0.1f;
            m.startSizeY = sizeY;
        }
        if (growTime > 0) {
            growTime -= Time.fixedDeltaTime;
            return;
        }
        if (moveSpeed < attractMaxSpeed)
            moveSpeed += attractAcceleration;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed);
        if (Vector2.Distance(transform.position, player.position) < 0.05 * playerScript.fireSize) {
            playerScript.Grow();
            Destroy(gameObject);
        }
    }
}
