using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class BlockBehavior : MonoBehaviour
{
    public Vector3 rotationPoint;
    public bool isActivePiece = true;
    private float previousTime;
    public static float fallTime = 1f;
    public float quickFallTime = 0.1f;
    public static int height = 13;
    public static int width = 6;
    public Sprite[] spriteList;
    public static Transform[,] grid = new Transform[width, height];
    private List<Transform> toDeleteList = new List<Transform>();
    public bool isInitialBlock;
    public static int piecesNumber = 4;
    int colorsNumber = 1; //solo con cambiar este numero el juego cambia de 1 a 2 colores
    bool checkColor = true;
    bool canMove = true;
    private float lastDpadH;
    private float lastDpadV;
    public int playerBelongs;
    private int numberOfPlayers;
    public static int score;
    public static int comboCount = 1;
    public static int scorePerPiece = 30;
    public static int currentLevel = 0;
    public static int nextScoreToLevelUp=120;
    public static int[] levelUpScores = {180,800,4000,7000,10000,13000,18000,22000,26000,999999999};
    public static List<Vector2Int> piecesList = new List<Vector2Int> {new Vector2Int(2,0), new Vector2Int(1,0) };
    private PlayerControls controls1;
    private Vector2 moveInput1;
    private bool fallQuickly = false;
    private bool fallQuickly1 = false;

    private void Awake()
    {
        controls1 = new PlayerControls();
    }


    void Start()
    {
        
        foreach (Transform children in transform)
        {
            int randomIndex = Random.Range(0, piecesList.Count);
            Vector2Int randomPiece = piecesList[randomIndex];
            children.GetComponent<PieceBehavior>().type = randomPiece[0];
            children.GetComponent<PieceBehavior>().color = randomPiece[1];
            children.GetComponent<SpriteRenderer>().sprite = spriteList[randomPiece[0] + piecesNumber * randomPiece[1]];
        }
        numberOfPlayers = FindObjectOfType<GameController>().numberOfPlayers;
        if (!isInitialBlock) this.enabled = false;

        if (isInitialBlock) 
        { 
            if(numberOfPlayers==1)
            {
                Destroy(gameObject);

                FindObjectOfType<GameController>().SpawnNewBlock(1);
            }
            else
            {
                playerBelongs = Random.Range(0, numberOfPlayers);
            }
        }

        //todo esto no parece funcionar
        /*
        var gamepads = Gamepad.all;
       
        if (gamepads.Count > 1)
        {
            Debug.Log("mas de 1 mando");
            if (playerBelongs == 0)
            {
                controls1.devices = new InputDevice[] { gamepads[0] };
                controls1.Enable();
            }
            if (playerBelongs == 1)
            {
                
                {
                    controls1.devices = new InputDevice[] { gamepads[1] };
                    controls1.Enable();
                }

            }
        }
        */

            
    }
    private void OnEnable()
    {
        controls1.Player1.Enable();
    }

    private void OnDisable()
    {
        controls1.Player1.Disable();
    }

    private void OnMove(InputValue inputValue)
    {
        float direction = inputValue.Get<float>();
        if (playerBelongs == 0)
        {
            if (canMove)
            {
                transform.position = transform.position + new Vector3(direction, 0, 0);
                if (!VaildMove())
                {
                    transform.position = transform.position - new Vector3(direction, 0, 0);
                }

            }
        }
    }

    public void OnDrop(InputValue value)
    {
        if (playerBelongs == 0)
        { 
            // We assume that a value greater than 0 indicates the button is pressed.
            float buttonValue = value.Get<float>();
            fallQuickly = (buttonValue > 0.5f);
        }
    }


    private void OnRotate()
    {
        if (playerBelongs == 0)
        {
            if (canMove)
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                if (!VaildMove())
                {
                    if (transform.rotation.eulerAngles == (new Vector3(0, 0, 0)))
                    {
                        transform.position = transform.position + new Vector3(-1, 0, 0);
                        if (!VaildMove())
                        {
                            transform.position += new Vector3(1, 0, 0);
                            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                        }
                        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                    }
                    if (transform.rotation.eulerAngles == (new Vector3(0, 0, 180)))
                    {
                        transform.position = transform.position + new Vector3(1, 0, 0);
                        if (!VaildMove())
                        {
                            transform.position -= new Vector3(1, 0, 0);
                            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                        }
                        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                    }

                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                }
                else { FindObjectOfType<GameController>().PlayRotateSound(); }
            }
        }
    }

    private void OnMove1(InputValue inputValue)
    {
        float direction = inputValue.Get<float>();
        if (playerBelongs == 1)
        {
            if (canMove)
            {
                transform.position = transform.position + new Vector3(direction, 0, 0);
                if (!VaildMove())
                {
                    transform.position = transform.position - new Vector3(direction, 0, 0);
                }

            }
        }
    }

    public void OnDrop1(InputValue value)
    {
        if (playerBelongs == 1)
        {
            // We assume that a value greater than 0 indicates the button is pressed.
            float buttonValue = value.Get<float>();
            fallQuickly1 = (buttonValue > 0.5f);
        }
    }


    private void OnRotate1()
    {
        if (playerBelongs == 1)
        {
            if (canMove)
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                if (!VaildMove())
                {
                    if (transform.rotation.eulerAngles == (new Vector3(0, 0, 0)))
                    {
                        transform.position = transform.position + new Vector3(-1, 0, 0);
                        if (!VaildMove())
                        {
                            transform.position += new Vector3(1, 0, 0);
                            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                        }
                        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                    }
                    if (transform.rotation.eulerAngles == (new Vector3(0, 0, 180)))
                    {
                        transform.position = transform.position + new Vector3(1, 0, 0);
                        if (!VaildMove())
                        {
                            transform.position -= new Vector3(1, 0, 0);
                            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                        }
                        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                    }

                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                }
                else { FindObjectOfType<GameController>().PlayRotateSound(); }
            }
        }
    }

    void Update()
    {
        if (Time.time - previousTime > (((playerBelongs ==0 && fallQuickly)||(playerBelongs == 1 && fallQuickly1) ) ? quickFallTime : fallTime))
        {
            bool continueCheckingBreakers = false;
            bool continueCheckingDowners = false;


            foreach (Transform children in transform)
            {
                int roundedX = Mathf.RoundToInt(children.transform.position.x);
                int roundedY = Mathf.RoundToInt(children.transform.position.y);
                if (roundedY>0&&roundedY<height)
                grid[roundedX, roundedY] = null;
            }
            transform.position += new Vector3(0, -1f, 0);

            if (!VaildMove())
            {
                transform.position -= new Vector3(0, -1f, 0);
                AddToGridBlock(transform);
                canMove = false;
                continueCheckingDowners = CheckForDowners();

                if (!continueCheckingDowners)
                {
                    CheckForBreakers();
                    continueCheckingBreakers = DeleteList();
                }


                if (!continueCheckingDowners && !continueCheckingBreakers)
                {
                    this.enabled = false;
                    FindObjectOfType<GameController>().SpawnNewBlock(playerBelongs*(-1)+1);

                }
            }
            previousTime = Time.time;
        }
    }
    void AddToGridBlock(Transform blockTransform)
    {
        foreach (Transform children in blockTransform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            grid[roundedX, roundedY] = children;
            children.transform.rotation = Quaternion.identity;
        }
    }
    void AddToGridPiece(Transform blockTransform)
    {

        int roundedX = Mathf.RoundToInt(blockTransform.transform.position.x);
        int roundedY = Mathf.RoundToInt(blockTransform.transform.position.y);
        grid[roundedX, roundedY] = blockTransform;
        blockTransform.rotation = Quaternion.identity;

    }

    bool VaildMove()
    { if (transform.childCount == 0) { return false; }
        else
        {
            foreach (Transform children in transform)
            {
                int roundedX = Mathf.RoundToInt(children.transform.position.x);
                int roundedY = Mathf.RoundToInt(children.transform.position.y);

                if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
                {
                    return false;
                }
                if (grid[roundedX, roundedY] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void CheckForBreakers()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[i, j] != null)
                {
                    PieceBehavior pieceBehavior = grid[i, j].gameObject.GetComponent<PieceBehavior>();
                    if (pieceBehavior.type == 0)
                    {
                        if ((i + 1 < width && j + 2 < height))
                        {
                            {
                                if (grid[i + 1, j + 2] != null)
                                {
                                    if (grid[i + 1, j + 2].gameObject.GetComponent<PieceBehavior>().type == 0)
                                    {
                                        //
                                        Check7KnightJumps(1, 2, i, j);
                                    }
                                }
                            }
                        }
                        if ((i + 2 < width && j + 1 < height))
                        {
                            {
                                if (grid[i + 2, j + 1] != null)
                                {
                                    if (grid[i + 2, j + 1].gameObject.GetComponent<PieceBehavior>().type == 0)
                                    {
                                        //
                                        Check7KnightJumps(2, 1, i, j);
                                    }
                                }
                            }
                        }
                        if ((i + 2 < width && j + -1 >= 0))
                        {
                            {
                                if (grid[i + 2, j + -1] != null)
                                {
                                    if (grid[i + 2, j + -1].gameObject.GetComponent<PieceBehavior>().type == 0)
                                    {
                                        //
                                        Check7KnightJumps(2, -1, i, j);
                                    }
                                }
                            }
                        }
                        if ((i + 1 < width && j + -2 >= 0))
                        {
                            {
                                if (grid[i + 1, j + -2] != null)
                                {
                                    if (grid[i + 1, j + -2].gameObject.GetComponent<PieceBehavior>().type == 0)
                                    {
                                        //
                                        Check7KnightJumps(1, -2, i, j);
                                    }
                                }
                            }
                        }

                        if ((i + -1 >= 0 && j + 2 < height))
                        {
                            {
                                if (grid[i + -1, j + 2] != null)
                                {
                                    if (grid[i + -1, j + 2].gameObject.GetComponent<PieceBehavior>().type == 0)
                                    {
                                        //
                                        Check7KnightJumps(-1, 2, i, j);
                                    }
                                }
                            }
                        }
                        if ((i + -2 >= 0 && j + 1 < height))
                        {
                            {
                                if (grid[i + -2, j + 1] != null)
                                {
                                    if (grid[i + -2, j + 1].gameObject.GetComponent<PieceBehavior>().type == 0)
                                    {
                                        //
                                        Check7KnightJumps(-2, 1, i, j);
                                    }
                                }
                            }
                        }
                        if ((i + -2 >= 0 && j + -1 >= 0))
                        {
                            {
                                if (grid[i + -2, j + -1] != null)
                                {
                                    if (grid[i + -2, j + -1].gameObject.GetComponent<PieceBehavior>().type == 0)
                                    {
                                        //
                                        Check7KnightJumps(-2, -1, i, j);
                                    }
                                }
                            }
                        }
                        if ((i + -1 >= 0 && j + -2 >= 0))
                        {
                            {
                                if (grid[i + -1, j + -2] != null)
                                {
                                    if (grid[i + -1, j + -2].gameObject.GetComponent<PieceBehavior>().type == 0)
                                    {
                                        //
                                        Check7KnightJumps(-1, -2, i, j);
                                    }
                                }
                            }
                        }
                    }
                    else if (pieceBehavior.type == 1)
                    {
                        if ((i + 1 < width && j + 1 < height))
                        {
                            if (grid[i + 1, j + 1] != null)
                            {
                                if (grid[i + 1, j + 1].gameObject.GetComponent<PieceBehavior>().type == 1)
                                {
                                    if ((i + 2 < width && j + 2 < height))
                                    {
                                        if (grid[i + 2, j + 2] != null)
                                        {
                                            if (grid[i + 2, j + 2].gameObject.GetComponent<PieceBehavior>().type == 1)
                                            {
                                                if (
                                                    CompareColor(grid[i, j], grid[i + 1, j + 1], checkColor) &&
                                                    CompareColor(grid[i + 2, j + 2], grid[i + 1, j + 1], checkColor) &&
                                                    CompareColor(grid[i, j], grid[i + 2, j + 2], checkColor))
                                                {
                                                    AddToDeleteList(grid[i, j]);
                                                    AddToDeleteList(grid[i + 1, j + 1]);
                                                    AddToDeleteList(grid[i + 2, j + 2]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if ((i + 1 < width && j - 1 >= 0))
                        {
                            if (grid[i + 1, j - 1] != null)
                            {
                                if (grid[i + 1, j - 1].gameObject.GetComponent<PieceBehavior>().type == 1)
                                {
                                    if ((i + 2 < width && j - 2 >= 0))
                                    {
                                        if (grid[i + 2, j - 2] != null)
                                        {
                                            if (grid[i + 2, j - 2].gameObject.GetComponent<PieceBehavior>().type == 1)
                                            {
                                                if (CompareColor(grid[i, j], grid[i + 1, j - 1], checkColor) &&
                                                    CompareColor(grid[i + 2, j - 2], grid[i + 1, j - 1], checkColor) &&
                                                    CompareColor(grid[i, j], grid[i + 2, j - 2], checkColor))
                                                {
                                                    AddToDeleteList(grid[i, j]);
                                                    AddToDeleteList(grid[i + 1, j - 1]);
                                                    AddToDeleteList(grid[i + 2, j - 2]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (pieceBehavior.type == 2)
                    {
                        if (i + 1 < width)
                        {
                            if (grid[i + 1, j] != null)
                            {
                                if (grid[i + 1, j].gameObject.GetComponent<PieceBehavior>().type == 2)
                                {
                                    if (i + 2 < width)
                                    {
                                        if (grid[i + 2, j] != null)
                                        {
                                            if (grid[i + 2, j].gameObject.GetComponent<PieceBehavior>().type == 2)
                                            {
                                                if (CompareColor(grid[i, j], grid[i + 1, j], checkColor) &&
                                                    CompareColor(grid[i + 2, j], grid[i, j], checkColor) &&
                                                    CompareColor(grid[i + 1, j], grid[i + 2, j], checkColor))
                                                {
                                                    AddToDeleteList(grid[i, j]);
                                                    AddToDeleteList(grid[i + 1, j]);
                                                    AddToDeleteList(grid[i + 2, j]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (j + 1 < height)
                        {
                            if (grid[i, j + 1] != null)
                            {
                                if (grid[i, j + 1].gameObject.GetComponent<PieceBehavior>().type == 2)
                                {
                                    if (j + 2 < height)
                                    {
                                        if (grid[i, j + 2] != null)
                                        {
                                            if (grid[i, j + 2].gameObject.GetComponent<PieceBehavior>().type == 2)
                                            {
                                                if (CompareColor(grid[i, j], grid[i, j + 1], checkColor) &&
                                                   CompareColor(grid[i, j], grid[i, j + 2], checkColor) &&
                                                   CompareColor(grid[i, j + 1], grid[i, j + 2], checkColor))
                                                {
                                                    {
                                                        AddToDeleteList(grid[i, j]);
                                                        AddToDeleteList(grid[i, j + 1]);
                                                        AddToDeleteList(grid[i, j + 2]);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (pieceBehavior.type == 3)
                    {
                        if ((i + 1 < width && j + 1 < height))
                        {
                            if (grid[i + 1, j + 1] != null)
                            {
                                if (grid[i + 1, j + 1].gameObject.GetComponent<PieceBehavior>().type == 3)
                                {
                                    if ((i + 2 < width && j + 2 < height))
                                    {
                                        if (grid[i + 2, j + 2] != null)
                                        {
                                            if (grid[i + 2, j + 2].gameObject.GetComponent<PieceBehavior>().type == 3)
                                            {
                                                if (CompareColor(grid[i, j], grid[i + 1, j + 1], checkColor) &&
                                                   CompareColor(grid[i, j], grid[i + 2, j + 2], checkColor) &&
                                                   CompareColor(grid[i + 1, j + 1], grid[i + 2, j + 2], checkColor))
                                                {
                                                    AddToDeleteList(grid[i, j]);
                                                    AddToDeleteList(grid[i + 1, j + 1]);
                                                    AddToDeleteList(grid[i + 2, j + 2]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if ((i + 1 < width && j - 1 >= 0))
                        {
                            if (grid[i + 1, j - 1] != null)
                            {
                                if (grid[i + 1, j - 1].gameObject.GetComponent<PieceBehavior>().type == 3)
                                {
                                    if ((i + 2 < width && j - 2 >= 0))
                                    {
                                        if (grid[i + 2, j - 2] != null)
                                        {
                                            if (grid[i + 2, j - 2].gameObject.GetComponent<PieceBehavior>().type == 3)
                                            {
                                                if (CompareColor(grid[i, j], grid[i + 1, j - 1], checkColor) &&
                                                   CompareColor(grid[i, j], grid[i + 2, j - 2], checkColor) &&
                                                   CompareColor(grid[i + 1, j - 1], grid[i + 2, j - 2], checkColor))
                                                {
                                                    AddToDeleteList(grid[i, j]);
                                                    AddToDeleteList(grid[i + 1, j - 1]);
                                                    AddToDeleteList(grid[i + 2, j - 2]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (i + 1 < width)
                        {
                            if (grid[i + 1, j] != null)
                            {
                                if (grid[i + 1, j].gameObject.GetComponent<PieceBehavior>().type == 3)
                                {
                                    if (i + 2 < width)
                                    {
                                        if (grid[i + 2, j] != null)
                                        {
                                            if (grid[i + 2, j].gameObject.GetComponent<PieceBehavior>().type == 3)
                                            {
                                                if (CompareColor(grid[i, j], grid[i + 1, j], checkColor) &&
                                                   CompareColor(grid[i, j], grid[i + 2, j], checkColor) &&
                                                   CompareColor(grid[i + 1, j], grid[i + 2, j], checkColor))
                                                {
                                                    AddToDeleteList(grid[i, j]);
                                                    AddToDeleteList(grid[i + 1, j]);
                                                    AddToDeleteList(grid[i + 2, j]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (j + 1 < height)
                        {
                            if (grid[i, j + 1] != null)
                            {
                                if (grid[i, j + 1].gameObject.GetComponent<PieceBehavior>().type == 3)
                                {
                                    if (j + 2 < height)
                                    {
                                        if (grid[i, j + 2] != null)
                                        {
                                            if (grid[i, j + 2].gameObject.GetComponent<PieceBehavior>().type == 3)
                                            {
                                                if (CompareColor(grid[i, j], grid[i, j + 1], checkColor) &&
                                                   CompareColor(grid[i, j], grid[i, j + 2], checkColor) &&
                                                   CompareColor(grid[i, j + 1], grid[i, j + 2], checkColor))
                                                {
                                                    AddToDeleteList(grid[i, j]);
                                                    AddToDeleteList(grid[i, j + 1]);
                                                    AddToDeleteList(grid[i, j + 2]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (pieceBehavior.type == 4)
                    {
                        if (i + 1 < width)
                        {
                            if (grid[i + 1, j] != null)
                            {
                                if (grid[i + 1, j].gameObject.GetComponent<PieceBehavior>().type == 4)
                                {
                                    if (j + 1 < height)
                                    {
                                        if (grid[i + 1, j + 1] != null)
                                        {
                                            if (grid[i + 1, j+1].gameObject.GetComponent<PieceBehavior>().type == 4)
                                            {
                                                if (CompareColor(grid[i, j], grid[i + 1, j+1], checkColor) &&
                                                    CompareColor(grid[i + 1, j], grid[i, j], checkColor) &&
                                                    CompareColor(grid[i + 1, j], grid[i + 1, j+1], checkColor))
                                                {
                                                    AddToDeleteList(grid[i, j]);
                                                    AddToDeleteList(grid[i + 1, j]);
                                                    AddToDeleteList(grid[i + 1, j + 1]);
                                                }
                                            }
                                        }
                                        if (grid[i, j + 1] != null)
                                        {
                                            if (grid[i, j + 1].gameObject.GetComponent<PieceBehavior>().type == 4)
                                            {
                                                if (CompareColor(grid[i, j], grid[i, j + 1], checkColor) &&
                                                    CompareColor(grid[i + 1, j], grid[i, j], checkColor) &&
                                                    CompareColor(grid[i + 1, j], grid[i, j + 1], checkColor))
                                                {
                                                    AddToDeleteList(grid[i, j]);
                                                    AddToDeleteList(grid[i + 1, j]);
                                                    AddToDeleteList(grid[i, j + 1]);
                                                }
                                            }
                                        }
                                    }
                                    if (j - 1 >= 0)
                                    {
                                        if (grid[i + 1, j - 1] != null)
                                        {
                                            if (grid[i + 1, j - 1].gameObject.GetComponent<PieceBehavior>().type == 4)
                                            {
                                                if (CompareColor(grid[i, j], grid[i + 1, j - 1], checkColor) &&
                                                    CompareColor(grid[i + 1, j], grid[i, j], checkColor) &&
                                                    CompareColor(grid[i + 1, j], grid[i + 1, j - 1], checkColor))
                                                {
                                                    AddToDeleteList(grid[i, j]);
                                                    AddToDeleteList(grid[i + 1, j]);
                                                    AddToDeleteList(grid[i + 1, j - 1]);
                                                }
                                            }
                                        }
                                        if (grid[i, j - 1] != null)
                                        {
                                            if (grid[i, j - 1].gameObject.GetComponent<PieceBehavior>().type == 4)
                                            {
                                                if (CompareColor(grid[i, j], grid[i, j - 1], checkColor) &&
                                                    CompareColor(grid[i + 1, j], grid[i, j], checkColor) &&
                                                    CompareColor(grid[i + 1, j], grid[i, j - 1], checkColor))
                                                {
                                                    AddToDeleteList(grid[i, j]);
                                                    AddToDeleteList(grid[i + 1, j]);
                                                    AddToDeleteList(grid[i, j - 1]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        //quiz�s esta funci�n deber�a ser del GameController o algo, pero como tenemos es de q el grid es static, no se
        //loopear por todo el grid y preguntarle a cada pieza sus rules de breakeo (quiz�s llamarle a una funcion que tiene)
    }

    void AddToDeleteList(Transform toDeleteItem)
    {
        toDeleteList.Add(toDeleteItem);
    }

    void Check7KnightJumps(int x, int y, int x_prev, int y_prev)
    {
        if (-x != 1 || -y != 2)
        {
            if ((x + x_prev + 1 < width && y + y_prev + 2 < height))
            {
                if (grid[x + x_prev + 1, y + y_prev + 2] != null)
                {
                    if (grid[x + x_prev + 1, y + y_prev + 2].gameObject.GetComponent<PieceBehavior>().type == 0)
                    {
                        if (CompareColor(grid[x + x_prev, y + y_prev], grid[x + x_prev + 1, y + y_prev + 2], checkColor) &&
                            CompareColor(grid[x + x_prev, y + y_prev], grid[x_prev, y_prev], checkColor) &&
                            CompareColor(grid[x + x_prev + 1, y + y_prev + 2], grid[x_prev, y_prev], checkColor))
                        {
                            AddToDeleteList(grid[x + x_prev, y + y_prev]);
                            AddToDeleteList(grid[x + x_prev + 1, y + y_prev + 2]);
                            AddToDeleteList(grid[x_prev, y_prev]); 
                        }
                    }
                }
            }
        }
        if (-x != 2 || -y != 1)
        {
            if ((x + x_prev + 2 < width && y + y_prev + 1 < height))
            {
                if (grid[x + x_prev + 2, y + y_prev + 1] != null)
                {
                    if (grid[x + x_prev + 2, y + y_prev + 1].gameObject.GetComponent<PieceBehavior>().type == 0)
                    {
                        if (CompareColor(grid[x + x_prev, y + y_prev], grid[x + x_prev + 2, y + y_prev + 1], checkColor) &&
                            CompareColor(grid[x + x_prev, y + y_prev], grid[x_prev, y_prev], checkColor) &&
                            CompareColor(grid[x + x_prev + 2, y + y_prev + 1], grid[x_prev, y_prev], checkColor))
                        {
                            AddToDeleteList(grid[x + x_prev, y + y_prev]);
                            AddToDeleteList(grid[x + x_prev + 2, y + y_prev + 1]);
                            AddToDeleteList(grid[x_prev, y_prev]); 
                        }
                    }
                }
            }
        }
        if (-x != 2 || -y != -1)
        {
            if ((x + x_prev + 2 < width && y + y_prev + -1 >= 0))
            {
                if (grid[x + x_prev + 2, y + y_prev + -1] != null)
                {
                    if (grid[x + x_prev + 2, y + y_prev + -1].gameObject.GetComponent<PieceBehavior>().type == 0)
                    {
                        if (CompareColor(grid[x + x_prev, y + y_prev], grid[x + x_prev + 2, y + y_prev - 1], checkColor) &&
                            CompareColor(grid[x + x_prev, y + y_prev], grid[x_prev, y_prev], checkColor) &&
                            CompareColor(grid[x + x_prev + 2, y + y_prev - 1], grid[x_prev, y_prev], checkColor))
                        {
                            AddToDeleteList(grid[x + x_prev, y + y_prev]);
                            AddToDeleteList(grid[x + x_prev + 2, y + y_prev + -1]);
                            AddToDeleteList(grid[x_prev, y_prev]); 
                        }
                    }
                }
            }
        }
        if (-x != 1 || -y != -2)
        {
            if ((x + x_prev + 1 < width && y + y_prev + -2 >= 0))
            {
                if (grid[x + x_prev + 1, y + y_prev + -2] != null)
                {
                    if (grid[x + x_prev + 1, y + y_prev + -2].gameObject.GetComponent<PieceBehavior>().type == 0)
                    {
                        if (CompareColor(grid[x + x_prev, y + y_prev], grid[x + x_prev + 1, y + y_prev - 2], checkColor) &&
                            CompareColor(grid[x + x_prev, y + y_prev], grid[x_prev, y_prev], checkColor) &&
                            CompareColor(grid[x + x_prev + 1, y + y_prev - 2], grid[x_prev, y_prev], checkColor))
                        {
                            AddToDeleteList(grid[x + x_prev, y + y_prev]);
                            AddToDeleteList(grid[x + x_prev + 1, y + y_prev + -2]);
                            AddToDeleteList(grid[x_prev, y_prev]); 
                        }
                    }
                }
            }
        }
        if (-x != -1 || -y != -2)
        {
            if ((x + x_prev + -1 >= 0 && y + y_prev + -2 >= 0))
            {
                if (grid[x + x_prev + -1, y + y_prev + -2] != null)
                {
                    if (grid[x + x_prev + -1, y + y_prev + -2].gameObject.GetComponent<PieceBehavior>().type == 0)
                    {
                        if (CompareColor(grid[x + x_prev, y + y_prev], grid[x + x_prev - 1, y + y_prev - 2], checkColor) &&
                            CompareColor(grid[x + x_prev, y + y_prev], grid[x_prev, y_prev], checkColor) &&
                            CompareColor(grid[x + x_prev - 1, y + y_prev - 2], grid[x_prev, y_prev], checkColor))
                        {
                            AddToDeleteList(grid[x + x_prev, y + y_prev]);
                            AddToDeleteList(grid[x + x_prev + -1, y + y_prev + -2]);
                            AddToDeleteList(grid[x_prev, y_prev]); 
                        }
                    }
                }
            }
        }
        if (-x != -2 || -y != -1)
        {
            if ((x + x_prev + -2 >= 0 && y + y_prev + -1 >= 0))
            {
                if (grid[x + x_prev + -2, y + y_prev + -1] != null)
                {
                    if (grid[x + x_prev + -2, y + y_prev + -1].gameObject.GetComponent<PieceBehavior>().type == 0)
                    {
                        if (CompareColor(grid[x + x_prev, y + y_prev], grid[x + x_prev - 2, y + y_prev - 1], checkColor) &&
                            CompareColor(grid[x + x_prev, y + y_prev], grid[x_prev, y_prev], checkColor) &&
                            CompareColor(grid[x + x_prev - 2, y + y_prev - 1], grid[x_prev, y_prev], checkColor))
                        {
                            AddToDeleteList(grid[x + x_prev, y + y_prev]);
                            AddToDeleteList(grid[x + x_prev + -2, y + y_prev + -1]);
                            AddToDeleteList(grid[x_prev, y_prev]); 
                        }
                    }
                }
            }
        }
        if (-x != -1 || -y != 2)
        {
            if ((x + x_prev + -1 >= 0 && y + y_prev + 2 < height))
            {
                if (grid[x + x_prev + -1, y + y_prev + 2] != null)
                {
                    if (grid[x + x_prev + -1, y + y_prev + 2].gameObject.GetComponent<PieceBehavior>().type == 0)
                    {
                        if (CompareColor(grid[x + x_prev, y + y_prev], grid[x + x_prev - 1, y + y_prev + 2], checkColor) &&
                            CompareColor(grid[x + x_prev, y + y_prev], grid[x_prev, y_prev], checkColor) &&
                            CompareColor(grid[x + x_prev - 1, y + y_prev + 2], grid[x_prev, y_prev], checkColor))
                        {
                            AddToDeleteList(grid[x + x_prev, y + y_prev]);
                            AddToDeleteList(grid[x + x_prev + -1, y + y_prev + 2]);
                            AddToDeleteList(grid[x_prev, y_prev]); 
                        }
                    }
                }
            }
        }
        if (-x != -2 || -y != 1)
        {
            if ((x + x_prev + -2 >= 0 && y + y_prev + 1 < height))
            {
                if (grid[x + x_prev + -2, y + y_prev + 1] != null)
                {
                    if (grid[x + x_prev + -2, y + y_prev + 1].gameObject.GetComponent<PieceBehavior>().type == 0)
                    {
                        if (CompareColor(grid[x + x_prev, y + y_prev], grid[x + x_prev - 2, y + y_prev + 1], checkColor) &&
                            CompareColor(grid[x + x_prev, y + y_prev], grid[x_prev, y_prev], checkColor) &&
                            CompareColor(grid[x + x_prev - 2, y + y_prev + 1], grid[x_prev, y_prev], checkColor))
                        {
                            AddToDeleteList(grid[x + x_prev, y + y_prev]);
                            AddToDeleteList(grid[x + x_prev + -2, y + y_prev + 1]);
                            AddToDeleteList(grid[x_prev, y_prev]); 
                        }
                    }
                }
            }
        }
    }

    bool DeleteList()
    {
        
        if (toDeleteList.Count > 0)
        {
            bool playerWin = false;
            HashSet<Transform> uniqueDeleteList = new HashSet<Transform>(toDeleteList);
            int scored = scorePerPiece * uniqueDeleteList.Count * comboCount;
            score = score + scored;
            FindObjectOfType<GameController>().UpdateScoredDisplay(scored);
            if (comboCount > 1)
            {
                FindObjectOfType<GameController>().ComboAnimation(comboCount);    
            }
            if (score >= levelUpScores[currentLevel])
            {
                LevelUp();
                FindObjectOfType<GameController>().DisplayLevelUp();
            }

            //xcomboCount Animation

            FindObjectOfType<GameController>().PlayDestroySound();
            for (int i = 0; i < toDeleteList.Count; i++)
            {
                int roundedX = Mathf.RoundToInt(toDeleteList[i].transform.position.x);
                int roundedY = Mathf.RoundToInt(toDeleteList[i].transform.position.y);
                grid[roundedX, roundedY] = null;
                if(numberOfPlayers ==2)
                {
                    if(toDeleteList[i].transform.CompareTag("Initial"))
                    {
                        playerWin = true;
                    }
                }
                Destroy(toDeleteList[i].gameObject);
                
                if(transform.childCount>0)
                {
                    foreach (Transform children in transform)
                    { 
                        children.SetParent(null);
                    }
                }

            }
            if (playerWin)
            {
                FindObjectOfType<GameController>().PlayerWins(playerBelongs);
            }
            toDeleteList.Clear();
            comboCount++;
            return true;
        }
        else { return false; }
    }

    bool CheckForDowners()
    {
        bool mustContinue = false;
        for (int i = 0; i < width; i++)
        {
            for (int j = 1; j < height; j++)
            {
                if (grid[i, j] != null)
                {
                    grid[i, j].position += new Vector3(0, -1, 0);
                    if (!ValidMoveAutomatic(grid[i, j]))
                    {
                        grid[i, j].position -= new Vector3(0, -1, 0);
                    }
                    else
                    {
                        AddToGridPiece(grid[i, j]);
                        grid[i, j] = null;
                        mustContinue = true;
                    }
                }
            }
        }
        return mustContinue;
    }



    bool ValidMoveAutomatic(Transform toCheckItem)
    {

        int roundedX = Mathf.RoundToInt(toCheckItem.transform.position.x);
        int roundedY = Mathf.RoundToInt(toCheckItem.transform.position.y);

        if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
        {
            return false;
        }
        if (grid[roundedX, roundedY] != null)
        {
            return false;
        }

        return true;
    }

    public void LevelUp()
    {
        if (numberOfPlayers==1)
        {
            currentLevel++;
            if (currentLevel == 1)
            {
                piecesList.Add(new Vector2Int(3, 0));
            }
            else if (currentLevel == 2)
            {
                piecesList.Add(new Vector2Int(0, 0));
            }
            else if (currentLevel == 3)
            {
                piecesList.Add(new Vector2Int(3, 1));
            }
            else if (currentLevel == 4)
            {
                piecesList.Add(new Vector2Int(1, 1));
            }
            else if (currentLevel == 5)
            {
                piecesList.Add(new Vector2Int(2, 1));
            }
            else if (currentLevel == 6)
            {
                piecesList.Add(new Vector2Int(0, 1));
            }
            else if (currentLevel == 7)
            {
                fallTime = 0.8f;

            }
            else if (currentLevel == 8)
            {
                fallTime = 0.6f;

            }
            else if (currentLevel == 9)
            {
                fallTime = 0.54f;
            } 
        }

    }

    bool CompareColor(Transform grid1, Transform grid2, bool checkcolor)
    {
        if (!checkcolor)
        { return true; }
        else if ((grid1.gameObject.GetComponent<PieceBehavior>().color == grid2.gameObject.GetComponent<PieceBehavior>().color) ||
        (grid1.gameObject.GetComponent<PieceBehavior>().color == 99 || grid2.gameObject.GetComponent<PieceBehavior>().color == 99))
        { return true; }
        else return false;
    }
}