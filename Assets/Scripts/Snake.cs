using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridPosotion;
    private Vector2Int gridMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimerMax;

    private void Awake()
    {
        gridPosotion = new Vector2Int(-5, 0);
        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gridPosotion.y += 1;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gridPosotion.y -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gridPosotion.x -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) 
        { 
            gridPosotion.x += 1; 
        }

        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridPosotion += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;
        }

        transform.position = new Vector3(gridPosotion.x, gridPosotion.y);
    }
}
