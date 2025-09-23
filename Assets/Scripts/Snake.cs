using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private enum Direction
    {
        Left,Right,Up,Down
    }

    public float snakeMoveSpeed = 2f;
    private Vector2Int gridPosotion;
    //private Vector2Int gridMoveDirection;
    private Direction gridMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    //[SerializeField] private Sprite snakeBody;
    //private List<Vector2Int> snakeMovePositionList;
    private List<SnakeMovePosition> snakeMovePositionList;
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
        //gridMoveDirection = new Vector2Int(1, 0);
        gridMoveDirection = Direction.Right;

        //snakeMovePositionList = new List<Vector2Int>();
        snakeMovePositionList = new List<SnakeMovePosition>();
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
            if (/*gridMoveDirection.y != -1*/ gridMoveDirection != Direction.Down)
            {
                //gridMoveDirection.x = 0;
                //gridMoveDirection.y = +1;
                gridMoveDirection = Direction.Up;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (/*gridMoveDirection.y != +1*/ gridMoveDirection != Direction.Up)
            {
                //gridMoveDirection.x = 0;
                //gridMoveDirection.y = -1;
                gridMoveDirection = Direction.Down;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (/*gridMoveDirection.x != +1*/ gridMoveDirection != Direction.Right)
            {
                //gridMoveDirection.x = -1;
                //gridMoveDirection.y = 0;
                gridMoveDirection = Direction.Left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (/*gridMoveDirection.x != -1*/ gridMoveDirection != Direction.Left)
            {
                //gridMoveDirection.x = +1;
                //gridMoveDirection.y = 0;
                gridMoveDirection = Direction.Right;
            }
        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime * snakeMoveSpeed;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(gridPosotion, gridMoveDirection);
            //snakeMovePositionList.Insert(0, gridPosotion);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection)
            {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector2Int(+1, 0); break;
                case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, +1); break;
                case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }

            //gridPosotion += gridMoveDirection;
            gridPosotion += gridMoveDirectionVector;


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

            //transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection));
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector));

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
            //snakeBodyPartList[i].SetGridPosition(snakeMovePositionList[i]);
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
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
        foreach(SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        //gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }

    // Handles a Single Body Part
    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up: angle = 90; break;
                case Direction.Down: angle = 90; break;
                case Direction.Left: angle = 0; break;
                case Direction.Right: angle = 0; break;
            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    // Handles one move position from the Snake 
    private class SnakeMovePosition
    {
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(Vector2Int gridPosition, Direction direction)
        {
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }
    }
}
