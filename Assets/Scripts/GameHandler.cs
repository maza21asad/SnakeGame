using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject snakeHeadGameObject = new GameObject();
        SpriteRenderer snakeSpriteRanderer = snakeHeadGameObject.AddComponent<SpriteRenderer>();
        snakeSpriteRanderer.sprite = GameAssets.Instance.snakeHeadSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
