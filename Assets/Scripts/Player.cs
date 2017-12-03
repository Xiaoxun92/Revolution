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
    public float fireInitSize;
    [Header("每次吸收增长量")]
    public float sizeGrowSpeed;
    [Header("每秒衰减速度")]
    public float sizeDropSpeed;
    [Header("火焰熄灭大小")]
    public float fireMinSize;
    [Header("火焰进阶大小")]
    public float[] fireMaxSize;

    [Header("视野初始大小")]
    public float visionInitSize;
    [Header("视野变化速度")]
    public float visionIncreaseSpeed;

    GameManager gameManager;
    Transform fireTransform;
    Transform lightTransform;
    Vision visionScript;

    public bool burning;

    public float igniteRadius;
    int igniteCount;
    float lastIgniteTime;

    public float fireSize;
    Vector2 currentSpeed;

    // Debug
    public bool lockSize = false;

    void Start()
    {
        gameManager = Camera.main.GetComponent<GameManager>();
        fireTransform = transform.GetChild(0);
        lightTransform = transform.GetChild(1);
        visionScript = GameObject.Find("Vision").GetComponent<Vision>();

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
            return;
        }

        if (fireSize <= fireMinSize) {
            FireOut();
            return;
        }

        if (fireSize >= fireMaxSize[gameManager.gameState]) {
            gameManager.NextStage();
            return;
        }

        if (lockSize == false)
            fireSize -= sizeDropSpeed * Time.deltaTime;
        fireTransform.localScale = Vector3.one * fireSize;
        igniteRadius = fireSize * 0.75f;
        lightTransform.localScale = Vector3.Lerp(lightTransform.localScale, Vector3.one * igniteRadius * 2, 0.05f);
        visionScript.radius = Mathf.Lerp(visionScript.radius, (fireSize - fireInitSize) * visionIncreaseSpeed + visionInitSize, 0.05f);
    }

    // Movement control
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

        if (Time.time - lastIgniteTime > igniteGapMax)
            igniteCount = 1;
        else
            igniteCount++;

        lastIgniteTime = Time.time;
        Instantiate(igniteFlashPrefab);

        // Start fire
        if (igniteCount == igniteCountNeeded) {
            burning = true;
            fireSize = fireInitSize;
            fireTransform.gameObject.SetActive(true);
            lightTransform.gameObject.SetActive(true);
            visionScript.radius = visionInitSize;
        }
    }

    void FireOut()
    {
        burning = false;
        igniteCount = 0;
        lastIgniteTime = 0;
        fireTransform.gameObject.SetActive(false);
        lightTransform.gameObject.SetActive(false);
        visionScript.gameObject.SetActive(false);

        foreach (GameObject civ in GameObject.FindGameObjectsWithTag("Civilian")) {
            if (civ.GetComponent<Civilian>().burning)
                Destroy(civ);
        }
    }

    public void Grow()
    {
        fireSize += sizeGrowSpeed;
    }
}
