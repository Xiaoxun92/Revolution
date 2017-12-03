using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpace : MonoBehaviour {

    public Transform fire;
    public float speedScaleX;
    public float speedScaleY;

    Vector2 startPosition;

    void Update () {
        transform.position = new Vector2(fire.position.x * speedScaleX, fire.position.y * speedScaleY);
	}
}
