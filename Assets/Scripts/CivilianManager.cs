using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianManager : MonoBehaviour {

    public Transform player;
    public GameObject civilianPrefab;
    public float tileLength;
    [Header("平民的生成密度")]
    public int civilianPerTile;

    int playerX;
    int playerY;

    void Start () {
        playerX = 0;
        playerY = 0;

        CreateCivilian(true, playerX - 1);
        CreateCivilian(true, playerX);
        CreateCivilian(true, playerX + 1);
    }
	
	void Update ()
    {
        int newPlayerX = Mathf.FloorToInt(player.position.x / tileLength);
        int newPlayerY = Mathf.FloorToInt(player.position.y / tileLength);

        if (newPlayerX > playerX) {
            playerX = newPlayerX;
            CreateCivilian(true, playerX + 1);
            DestroyCivilian(true, playerX - 2);
        } else if (newPlayerX < playerX) {
            playerX = newPlayerX;
            CreateCivilian(true, playerX - 1);
            DestroyCivilian(true, playerX + 2);
        } else if (newPlayerY > playerY) {  // Don't change x and y at the same frame
            playerY = newPlayerY;
            CreateCivilian(false, playerY + 1);
            DestroyCivilian(false, playerY - 2);
        } else if (newPlayerY < playerY) {
            playerY = newPlayerY;
            CreateCivilian(false, playerY - 1);
            DestroyCivilian(false, playerY + 2);
        }
    }

    void CreateCivilian(bool xIsNew, int coord)
    {
        for (int offset = -1; offset < 2; offset++) {
            for (int j = 0; j < civilianPerTile; j++) {

                Vector2 randomPos = new Vector2();
                if (xIsNew)
                    randomPos = new Vector2(coord, playerY + offset) * tileLength;
                else
                    randomPos = new Vector2(playerX + offset, coord) * tileLength;
                randomPos += new Vector2(Random.value * tileLength, Random.value * tileLength);

                Instantiate(civilianPrefab, randomPos, Quaternion.identity, transform);
            }
        }
    }

    void DestroyCivilian(bool xIsNew, int coord)
    {
        foreach (Transform civ in transform) {
            if (xIsNew) {
                if (Mathf.FloorToInt(civ.position.x / tileLength) == coord)
                    Destroy(civ.gameObject);
            } else {
                if (Mathf.FloorToInt(civ.position.y / tileLength) == coord)
                    Destroy(civ.gameObject);
            }
        }
    }
}
