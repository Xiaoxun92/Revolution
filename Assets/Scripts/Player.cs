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
    public GameObject igniteFlashPrefab;

    [Header("【鼠标控制方案】")]
    [Header("最大移动速度")]
    public float maxSpeed1;
    [Header("加速度")]
    public float acceleration1;
    [Header("摩擦减速比例")]
    public float drag1;

    [Header("【键盘控制方案】")]
    [Header("最大移动速度")]
    public float maxSpeed2;
    [Header("加速度")]
    public float acceleration2;
    [Header("摩擦减速比例")]
    public float drag2;

    [Header("火焰初始大小")]
    public float fireSize;
    [Header("每次吸收增长量")]
    public float sizeGrowSpeed;
    [Header("每秒衰减速度")]
    public float sizeDropSpeed;
    [Header("火焰进阶大小")]
    public float fireMaxSize;

    public float igniteRadius;

    GameManager gameManager;

    public bool burning;
    int igniteCount;
    float lastIgniteTime;
    Vector2 currentSpeed;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        burning = false;
        igniteCount = 0;
        lastIgniteTime = 0;
        currentSpeed = new Vector2();
    }

    void Update()
    {
        if (burning == false) {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
                Ignite();
            }
        } else {
            if (fireSize >= fireMaxSize)
                gameManager.NextStage();
            fireSize -= sizeDropSpeed * Time.deltaTime;
            transform.GetChild(0).localScale = new Vector3(fireSize, fireSize, 1);
        }
    }

    // Handle player controls
    void FixedUpdate()
    {
        if (burning == false)
            return;

        // Update speed and position
        Vector2 controlVector = Vector2.zero;

        if (gameManager.mouseControlMode) {
            if (Input.GetMouseButton(0)) {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                controlVector = mousePos - transform.position;
            }
        } else {
            if (Input.GetKey(KeyCode.W))
                controlVector.y += 1;
            if (Input.GetKey(KeyCode.S))
                controlVector.y -= 1;
            if (Input.GetKey(KeyCode.A))
                controlVector.x -= 1;
            if (Input.GetKey(KeyCode.D))
                controlVector.x += 1;
        }

        controlVector = controlVector.normalized;

        float maxSpeed = 0;
        float acceleration = 0;
        float drag = 0;
        if (gameManager.mouseControlMode) {
            maxSpeed = maxSpeed1;
            acceleration = acceleration1;
            drag = drag1;
        } else {
            maxSpeed = maxSpeed2;
            acceleration = acceleration2;
            drag = drag2;
        }

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
        Instantiate(igniteFlashPrefab, transform);
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
