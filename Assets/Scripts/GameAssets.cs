using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance;

    public Sprite snakeHeadSprite;
    public Sprite redAppleSprite;

    private void Awake()
    {
        Instance = this;
    }
}
