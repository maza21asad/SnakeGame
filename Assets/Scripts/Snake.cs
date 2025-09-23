using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public float snakeMoveSpeed = 2f;
    private Vector2Int gridPosotion;
    private Vector2Int gridMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    //[SerializeField] private Sprite snakeBody;
    private List<Vector2Int> snakeMovePositionList;
    //private List<Transform> snakeBodyTransfoemList;
    private List<SnakeBodyPart> snakeBodyPartList;

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosotion = new Vector2Int(-5, 0);
        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);

        snakeMovePositionList = new List<Vector2Int>();
        snakeBodySize = 0;

        //snakeBodyTransfoemList = new List<Transform>();
        snakeBodyPartList = new List<SnakeBodyPart>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleGridMovement();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection.y != -1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection.y != +1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection.x != +1)
            {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection.x != -1)
            {
                gridMoveDirection.x = +1;
                gridMoveDirection.y = 0;
            }
        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime * snakeMoveSpeed;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            snakeMovePositionList.Insert(0, gridPosotion);

            gridPosotion += gridMoveDirection;

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosotion);
            if (snakeAteFood)
            {
                // Snake ate food and grow body
                snakeBodySize++;
                CreateSnakeBody();
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            //for (int i = 0; i < snakeMovePositionList.Count; i++)
            //{
            //    Vector2Int snakeMovePosition = snakeMovePositionList[i];
            //    //World_Sprite worldSprite = World_Sprite.Create(new Vector3(snakeMovePosition.x, snakeMovePosition.y), Vector3.one * .5f, Color.white);
            //    //FunctionTimer.Create(worldSprite.DestroySelf, gridMoveTimerMax);

            //    GameObject bodyPart = new GameObject("SnakeBodyPart", typeof(SpriteRenderer));

            //    // Assign sprite
            //    SpriteRenderer sr = bodyPart.GetComponent<SpriteRenderer>();
            //    sr.sprite = snakeBody;
            //    sr.sortingOrder = -1; // so it renders behind the head

            //    // Position it
            //    bodyPart.transform.position = new Vector3(snakeMovePosition.x, snakeMovePosition.y, 0f);

            //    // Destroy it after one move (like FunctionTimer did)
            //    Destroy(bodyPart, gridMoveTimerMax);
            //}

            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection));
            transform.position = new Vector3(gridPosotion.x, gridPosotion.y);

            UpdateSnakeBodyParts();

            //for (int i = 0; i < snakeBodyTransfoemList.Count; i++)
            //{
            //    Vector3 snakeBodyPosition = new Vector3(snakeMovePositionList[i].x, snakeMovePositionList[i].y);
            //    snakeBodyTransfoemList[i].position = snakeBodyPosition;
            //}
        }       
    }

    //private void CreateSnakeBody()
    //{
    //    GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
    //    snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.snakeBodySprite;
    //    snakeBodyTransfoemList.Add(snakeBodyGameObject.transform);
    //    snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = - snakeBodyTransfoemList.Count;
    //}

    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            //Vector3 snakeBodyPosition = new Vector3(snakeMovePositionList[i].x, snakeMovePositionList[i].y);
            snakeBodyPartList[i].SetGridPosition(snakeMovePositionList[i]);
        }
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosotion;
    }

    // Return the full list of positions occupied by the snake; Head + Body
    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosotion };
        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }

    private class SnakeBodyPart
    {
        private Vector2Int gridPosition;
        private Transform transform;
        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetGridPosition(Vector2Int gridPosition)
        {
            this.gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
        }
    }
}
