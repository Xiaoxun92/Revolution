using UnityEngine;

public class Map : MonoBehaviour {

    public float tileWidth;
    public float tileHeight;
	
	void Update () {
        Vector2 cameraPos = Camera.main.transform.position;

        if (cameraPos.x > transform.position.x + tileWidth)
            transform.position = new Vector2(transform.position.x + tileWidth * 2, transform.position.y);
        else if (cameraPos.x < transform.position.x - tileWidth)
            transform.position = new Vector2(transform.position.x - tileWidth * 2, transform.position.y);

        if (cameraPos.y > transform.position.y + tileWidth)
            transform.position = new Vector2(transform.position.x, transform.position.y + tileHeight * 2);
        else if (cameraPos.y < transform.position.y - tileWidth)
            transform.position = new Vector2(transform.position.x, transform.position.y - tileHeight * 2);
    }
}