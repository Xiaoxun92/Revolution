using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool mouseControlMode;
    public int gameState;
    float cameraSize;
    
	void Start () {
        gameState = 0;
        cameraSize = 5;
        Debug.Log(cameraSize);
    }
	
	void Update () {
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, cameraSize, 0.01f);
        Debug.Log(Camera.main.orthographicSize);
	}

    public void NextStage()
    {
        gameState++;
        Debug.Log(gameState);
        switch (gameState) {
            case 1:
                cameraSize = 15;
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
