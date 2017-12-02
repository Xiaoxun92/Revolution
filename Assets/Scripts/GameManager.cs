using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool mouseControlMode;
    public GameObject map;
    public float[] cameraSize;
    public int gameState;
    
	void Start () {
        NextStage();
    }
	
	void Update () {
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, cameraSize[gameState], 0.04f);
	}

    public void NextStage()
    {
        gameState++;
        map.GetComponent<Map>().OnGameStateChange(gameState);
        switch (gameState) {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
