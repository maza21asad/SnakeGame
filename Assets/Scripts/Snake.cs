using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private enum Direction { Left,Right,Up,Down }
    private enum State { Alive, Dead}

    private State state;
    public float snakeMoveSpeed = 2f;
    private Vector2Int gridPosotion;

    private Direction gridMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;

    private List<SnakeMovePosition> snakeMovePositionList;

    private List<SnakeBodyPart> snakeBodyPartList;

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosotion = new Vector2Int(10, 10);
        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;

        gridMoveDirection = Direction.Right;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();
        state = State.Alive;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Alive: HandleInput(); HandleGridMovement(); break;
            case State.Dead: break;
        }
        
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left)
            {
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

            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosotion, gridMoveDirection);
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

            gridPosotion += gridMoveDirectionVector;

            gridPosotion = levelGrid.ValidateGridPosition(gridPosotion);

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

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosotion == snakeBodyPartGridPosition)
                {
                    // Game Over
                    Debug.Log("Game Over");
                    Debug.Log("Snake eat itself");
                    state = State.Dead;
                }
            }

            transform.position = new Vector3(gridPosotion.x, gridPosotion.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector));

            UpdateSnakeBodyParts();
        }       
    }

    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count,
            GameAssets.Instance.snakeBodySprite,
            GameAssets.Instance.snakeBodyCornerSprite));
    }


    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
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
        return gridPositionList;
    }

    // Handles a Single Body Part
    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        private Sprite straightSprite;
        private Sprite cornerSprite;

        public SnakeBodyPart(int bodyIndex, Sprite straightSprite, Sprite cornerSprite)
        {
            this.straightSprite = straightSprite;
            this.cornerSprite = cornerSprite;

            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = straightSprite; // default
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(
                snakeMovePosition.GetGridPosition().x,
                snakeMovePosition.GetGridPosition().y,
                0f);

            Direction dir = snakeMovePosition.GetDirection();
            Direction prevDir = snakeMovePosition.GetPreviousDirection();

            SpriteRenderer sr = transform.GetComponent<SpriteRenderer>();
            float angle = 0f;

            if (dir == prevDir)
            {
                // ---- STRAIGHT BODY ----
                sr.sprite = straightSprite;
                switch (dir)
                {
                    case Direction.Up: angle = 90; break;
                    case Direction.Down: angle = 90; break;
                    case Direction.Left: 
                    case Direction.Right: angle = 0; break;
                }
            }
            else
            {
                // ---- CORNER BODY ----
                sr.sprite = cornerSprite;

                switch (dir)
                {
                    case Direction.Up:
                        switch (prevDir)
                        {
                            case Direction.Left: angle = 90; break;   
                            case Direction.Right: angle = 180; break;   
                        }
                        break;

                    case Direction.Down:
                        switch (prevDir)
                        {
                            case Direction.Left: angle = 0; break;  
                            case Direction.Right: angle = 270; break;  
                        }
                        break;

                    case Direction.Left:
                        switch (prevDir)
                        {
                            case Direction.Up: angle = 270; break;   
                            case Direction.Down: angle = 180; break;  
                        }
                        break;

                    case Direction.Right:
                        switch (prevDir)
                        {
                            case Direction.Up: angle = 0; break;   
                            case Direction.Down: angle = 90; break;  
                        }
                        break;
                }
            }

            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }

        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition != null ? snakeMovePosition.GetGridPosition() : new Vector2Int(-999, -999);
        }
    }

    // Handles one move position from the Snake 
    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
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

        public Direction GetPreviousDirection()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            } 
            else
            {
                return previousSnakeMovePosition.direction;
            }           
        }
    }
}
