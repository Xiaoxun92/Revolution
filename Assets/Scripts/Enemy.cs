using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameManager gameManager;

    public float roamSpeed;
    public float attractSpeed;
    public float detectRadius;

    Transform player;
    Player playerScript;

    int timer;
    Vector3 direction;

    void Start()
    {
        gameManager = Camera.main.GetComponent<GameManager>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<Player>();

        timer = 1;
        direction = new Vector2();
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(transform.GetChild(4).position, player.position) < detectRadius) {
            transform.GetChild(0).gameObject.SetActive(true);
        } else {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        if (gameManager.stateChanging)
            return;

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
    }

    void SetAlpha(Transform child, float alpha)
    {
        Color c = child.GetComponent<SpriteRenderer>().color;
        c.a = alpha;
        child.GetComponent<SpriteRenderer>().color = c;
    }
}
