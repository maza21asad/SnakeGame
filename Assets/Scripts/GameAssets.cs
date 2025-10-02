using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    // Singleton Pattern
    public static GameAssets Instance;

    public Sprite snakeHeadSprite;
    public Sprite snakeBodySprite;
    public Sprite redAppleSprite;

    public Sprite snakeBodyCornerSprite;

    private void Awake()
    {
        Instance = this;
    }
}
