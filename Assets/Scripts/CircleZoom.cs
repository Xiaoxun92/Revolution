using UnityEngine;

public class CircleZoom : MonoBehaviour
{
    public float time1;
    public float time2;
    public float radius;
    public float alpha;

    float timer = 0;
    float rSpeed;
    float aSpeed;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = Random.Range(time1, time2);
            if (rSpeed >= 0) {
                rSpeed = -radius / timer / 100;
                aSpeed = -alpha / timer;
            } else {
                rSpeed = radius / timer / 100;
                aSpeed = alpha / timer;
            }
        }

        transform.localScale = transform.localScale + Vector3.one * rSpeed * Time.deltaTime;

        if (aSpeed != 0) {
            Color c = gameObject.GetComponent<SpriteRenderer>().color;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, c.a + aSpeed * Time.deltaTime);
        }
    }
}
