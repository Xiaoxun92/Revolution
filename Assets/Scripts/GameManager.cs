using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool mouseControlMode;
    public float fadeOutTime;
    public float fadeInTime;
    public float[] cameraSize;
    public Transform player;
    public Transform map;
    public Transform civManager;
    public GameObject rainLarge;

    public int gameState;
    public bool stateChanging;

    Transform transition;

    float timer;
    float zoomSpeed;

    void Start()
    {
        transition = transform.GetChild(0);

        gameState = 0;
        stateChanging = false;
        map.GetComponent<Map>().OnGameStateChange(gameState);
    }

    void Update()
    {
        if (stateChanging == false)
            return;

        Camera.main.orthographicSize += zoomSpeed * Time.deltaTime;
        transition.localScale = new Vector3(Camera.main.aspect, 1, 1) * Camera.main.orthographicSize * 2;

        // Fade out
        if (timer < 0) {
            timer += Time.deltaTime;
            SetAlpha(transition, Time.deltaTime / fadeOutTime);
            SetAlpha(civManager, -Time.deltaTime / fadeOutTime);
            SetAlpha(map, -Time.deltaTime / fadeOutTime);
            if (timer >= 0) {
                map.GetComponent<Map>().OnGameStateChange(gameState);
                switch (gameState) {
                    case 1:
                        foreach (Transform child in civManager)
                            Destroy(child.gameObject);
                        civManager.GetComponent<CivilianManager>().Init();
                        player.GetComponent<Player>().maxSpeed1 *= 2;
                        player.GetComponent<Player>().acceleration1 *= 2;
                        player.GetComponent<Player>().sizeGrowSpeed *= 2;
                        player.GetComponent<Player>().sizeDropSpeed *= 2;
                        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().Init();
                        break;
                    case 2:
                        foreach (Transform child in civManager)
                            Destroy(child.gameObject);
                        civManager.GetComponent<CivilianManager>().Init();
                        foreach (Transform child in GameObject.Find("EnemyManager").transform)
                            Destroy(child.gameObject);
                        rainLarge.SetActive(true);
                        break;
                    case 3:
                        break;
                }
            }
            return;
        }

        // Fade in
        timer += Time.deltaTime;
        SetAlpha(transition, -Time.deltaTime / fadeInTime);
        SetAlpha(civManager, Time.deltaTime / fadeInTime);
        SetAlpha(map, Time.deltaTime / fadeInTime);
        if (timer >= fadeInTime) {
            stateChanging = false;
        }
    }

    public void NextStage()
    {
        gameState++;
        stateChanging = true;
        timer = -fadeOutTime;
        zoomSpeed = (cameraSize[gameState] - Camera.main.orthographicSize) / (fadeOutTime + fadeInTime);
        switch (gameState) {
            case 1:
                GameObject.Find("Vision").SetActive(false);
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    void SetAlpha(Transform t, float delta)
    {
        if (t.GetComponent<SpriteRenderer>() != null) {
            Color c = t.GetComponent<SpriteRenderer>().color;
            c.a += delta;
            c.a = Mathf.Clamp01(c.a);
            t.GetComponent<SpriteRenderer>().color = c;
        }
        foreach (Transform child in t)
            SetAlpha(child, delta);
    }
}
