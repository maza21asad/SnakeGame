using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Snake snake;
    private LevelGrid LevelGrid;

    void Start()
    {
        LevelGrid = new LevelGrid(30, 15);

        snake.Setup(LevelGrid);
        LevelGrid.Setup(snake);
    }
}
