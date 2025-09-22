using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Snake snake;
    private LevelGrid LevelGrid;

    // Start is called before the first frame update
    void Start()
    {
        LevelGrid = new LevelGrid(5, 5);

        snake.Setup(LevelGrid);
        LevelGrid.Setup(snake);
    }
}
