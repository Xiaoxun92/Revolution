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


    public float growTime;
    public float attractMaxSpeed;
    public float attractAcceleration;

    SpriteRenderer sr;
    Transform player;
    Player playerScript;
    Color colorDelta;
    bool burning;
    float moveSpeed;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<Player>();
        colorDelta = (colorHot - colorCold) / igniteTime;
        burning = false;
        moveSpeed = 0;
    }

    void FixedUpdate()
    {
        if (burning == false) {
            if (playerScript.burning && Vector2.Distance(transform.position, player.position) < playerScript.fireSize * playerScript.igniteRadius) {
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
