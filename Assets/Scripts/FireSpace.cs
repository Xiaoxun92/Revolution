using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpace : MonoBehaviour {

    public Transform player;
    public float speedScaleX;
    public float speedScaleY;

    void Update () {
        transform.position = new Vector2(player.position.x * speedScaleX, player.position.y * speedScaleY);
	}
}
