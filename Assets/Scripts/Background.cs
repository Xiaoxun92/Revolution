using UnityEngine;

// Always covers all the screen
public class Background : MonoBehaviour {
	
	void Update () {
        float scale = Camera.main.orthographicSize;
        transform.localScale = new Vector2(scale * 4, scale * 2);
	}
}
