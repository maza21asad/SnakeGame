using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance;

    public Sprite snakeHeadSprite;

    private void Awake()
    {
        Instance = this;
    }
}
