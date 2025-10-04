using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    // Singleton Pattern
    public static GameAssets Instance;

    [Header("Snake Sprites")]
    public Sprite snakeHeadSprite;
    public Sprite snakeBodySprite;
    public Sprite snakeBodyCornerSprite;
    public Sprite snakeTailSprite;

    [Header("Apple Sprites")]
    public Sprite redAppleSprite;

    private void Awake()
    {
        Instance = this;
    }
}
