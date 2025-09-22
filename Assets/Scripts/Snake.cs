using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridPosotion;

    private void Awake()
    {
        gridPosotion = new Vector2Int(-5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(gridPosotion.x, gridPosotion.y);
    }
}
