using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpace : MonoBehaviour {

    public Transform fire;
    public float speedScaleX;
    public float speedScaleY;

    Vector2 startPosition;

    void Start()
    {
        startPosition = fire.position;
    }

    void Update () {
        transform.position = new Vector2((fire.position.x - startPosition.x) * speedScaleX, (fire.position.y - startPosition.y) * speedScaleY);
	}
}
