using UnityEngine;

public class Map : MonoBehaviour
{
    public Sprite[] sprites;
    public Sprite foreground;

    float tileWidth;
    float tileHeight;

    public void OnGameStateChange(int newState)
    {
        tileWidth = sprites[newState].bounds.extents.x * transform.localScale.x;
        tileHeight = sprites[newState].bounds.extents.y * transform.localScale.y;
        foreach (Transform child in transform) {
            child.GetComponent<SpriteRenderer>().sprite = sprites[newState];
            if (newState == 0)
                child.GetChild(0).GetComponent<SpriteRenderer>().sprite = foreground;
            else
                child.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        }
        transform.GetChild(0).localPosition = new Vector2(-tileWidth, tileHeight);
        transform.GetChild(1).localPosition = new Vector2(tileWidth, tileHeight);
        transform.GetChild(2).localPosition = new Vector2(-tileWidth, -tileHeight);
        transform.GetChild(3).localPosition = new Vector2(tileWidth, -tileHeight);

        transform.position = (Vector2)Camera.main.transform.position;
    }

    void Update()
    {
        // Update map location
        Vector2 cameraPos = Camera.main.transform.position;

        if (cameraPos.x > transform.position.x + tileWidth)
            transform.position = new Vector2(transform.position.x + tileWidth * 2, transform.position.y);
        else if (cameraPos.x < transform.position.x - tileWidth)
            transform.position = new Vector2(transform.position.x - tileWidth * 2, transform.position.y);

        if (cameraPos.y > transform.position.y + tileHeight)
            transform.position = new Vector2(transform.position.x, transform.position.y + tileHeight * 2);
        else if (cameraPos.y < transform.position.y - tileHeight)
            transform.position = new Vector2(transform.position.x, transform.position.y - tileHeight * 2);
    }
}