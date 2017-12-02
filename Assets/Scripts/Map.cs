using UnityEngine;

public class Map : MonoBehaviour
{
    public Sprite[] sprites;

    float tileWidth;
    float tileHeight;

    public void OnGameStateChange(int newState)
    {
        tileWidth = sprites[newState].bounds.extents.x * transform.localScale.x;
        tileHeight = sprites[newState].bounds.extents.y * transform.localScale.y;
        foreach (Transform child in transform) {
            child.GetComponent<SpriteRenderer>().sprite = sprites[newState];
        }
        transform.GetChild(0).position = new Vector2(-tileWidth, tileHeight);
        transform.GetChild(1).position = new Vector2(tileWidth, tileHeight);
        transform.GetChild(2).position = new Vector2(-tileWidth, -tileHeight);
        transform.GetChild(3).position = new Vector2(tileWidth, -tileHeight);
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