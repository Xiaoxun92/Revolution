using UnityEngine;

public class Vision : MonoBehaviour {

    public float radius;
    public float centerDefault;
	
	void Update () {
        transform.GetChild(0).localScale = Vector2.one * centerDefault * radius * 2;
        transform.GetChild(1).localPosition = Vector2.left * radius;
        transform.GetChild(2).localPosition = Vector2.right * radius;
        transform.GetChild(3).localPosition = Vector2.up * radius;
        transform.GetChild(4).localPosition = Vector2.down* radius;
    }
}
