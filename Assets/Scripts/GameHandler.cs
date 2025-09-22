using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private LevelGrid LevelGrid;

    // Start is called before the first frame update
    void Start()
    {
        LevelGrid = new LevelGrid(20, 10);
    }
}
