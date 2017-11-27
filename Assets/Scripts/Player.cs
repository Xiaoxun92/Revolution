using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("点燃玩家火焰需要的次数")]
    public int igniteCountNeeded;
    [Header("两次点燃的时间间隔")]
    public float igniteGapMin;
    public float igniteGapMax;
    public float igniteFlashLifespan;

    [Header("最大移动速度")]
    public float maxSpeed;
    [Header("加速度")]
    public float acceleration;
    [Header("摩擦减速比例")]
    public float drag;
    public float igniteRadius;
    public float sizeGrowSpeed;
    public float sizeDropSpeed;

    [Header("")]
    public bool burning;
    public float fireSize;
    int igniteCount;
    float lastIgniteTime;

    Vector2 currentSpeed;

    void Start()
    {
        burning = false;
        fireSize = 1;
        igniteCount = 0;
        lastIgniteTime = 0;

        currentSpeed = new Vector2();
    }

    void Update()
    {
        if (Time.time - lastIgniteTime >= igniteFlashLifespan)
            transform.GetChild(1).gameObject.SetActive(false);

        if (burning == false) {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
                Ignite();
            }
        } else {
            transform.GetChild(0).localScale = new Vector3(fireSize, fireSize, 1);
        }
    }

    void FixedUpdate()
    {
        if (burning == false)
            return;

        fireSize -= sizeDropSpeed;

        // Update speed and position
        Vector2 controlVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            controlVector.y += 1;
        if (Input.GetKey(KeyCode.S))
            controlVector.y -= 1;
        if (Input.GetKey(KeyCode.A))
            controlVector.x -= 1;
        if (Input.GetKey(KeyCode.D))
            controlVector.x += 1;

        if (Input.GetMouseButton(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            controlVector = mousePos - transform.position;
        }

        controlVector = controlVector.normalized;

        // Slow down previous speed
        Vector2 vVertical = Vector2.Dot(currentSpeed, controlVector) * controlVector;
        Vector2 vHorizontal = currentSpeed - vVertical;
        vHorizontal = Vector2.Lerp(vHorizontal, Vector2.zero, drag);
        currentSpeed = vVertical + vHorizontal;

        if (controlVector != Vector2.zero) {
            currentSpeed += controlVector * acceleration;
            currentSpeed = Vector2.ClampMagnitude(currentSpeed, maxSpeed);
        }

        transform.position += (Vector3)currentSpeed;
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void Ignite()
    {
        if (Time.time - lastIgniteTime < igniteGapMin)
            return;

        if (Time.time - lastIgniteTime < igniteGapMax)
            igniteCount++;
        else
            igniteCount = 1;
        lastIgniteTime = Time.time;
        transform.GetChild(1).gameObject.SetActive(true);
        if (igniteCount == igniteCountNeeded) {
            burning = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void Grow()
    {
        fireSize += sizeGrowSpeed;
    }
}
