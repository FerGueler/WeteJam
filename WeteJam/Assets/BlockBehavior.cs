using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    public Vector3 rotationPoint;
    public bool isActivePiece = true;
    private float previousTime;
    public float fallTime = 0.4f;
    public float quickFallTime = 0.05f;
    public static int height = 12;
    public static int width = 6;
    public Sprite[] spriteList;
    public static Transform[,] grid = new Transform[width, height];
    private List<Transform> toDeleteList = new List<Transform>();
    public bool isInitialBlock;
    int piecesNumber = 4;
    int colorsNumber = 2;
    bool checkColor = true;
    void Start()
    {
        foreach (Transform children in transform)
        {
            int r = Random.Range(0, piecesNumber);
            children.GetComponent<PieceBehavior>().type = r;
            int c = Random.Range(0, colorsNumber);
            children.GetComponent<PieceBehavior>().color = c;
            children.GetComponent<SpriteRenderer>().sprite = spriteList[r + piecesNumber * c];
        }
        if (!isInitialBlock) this.enabled = false;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick3Button2))
        {
            transform.position = transform.position + new Vector3(-1, 0, 0);
            if (!VaildMove())
            { transform.position -= new Vector3(-1, 0, 0); }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Joystick3Button1))
        {
            transform.position = transform.position + new Vector3(1, 0, 0);
            if (!VaildMove())
            { transform.position -= new Vector3(1, 0, 0); }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Joystick3Button3))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!VaildMove())
            { transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90); }
        }


        if (Time.time - previousTime > ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Joystick3Button0)) ? quickFallTime : fallTime))
        {
            bool continueChecking;

            transform.position += new Vector3(0, -1, 0);
            if (!VaildMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGridBlock(transform);
                do
                {
                    for (int i = 0; i < height; i++)
                    {
                        CheckForDowners();
                    }
                    CheckForBreakers();
                    continueChecking = DeleteList();
                } while (continueChecking);



                this.enabled = false;
                FindObjectOfType<GameController>().SpawnNewBlock();
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
        //quizás esta función debería ser del GameController o algo, pero como tenemos es de q el grid es static, no se
        //loopear por todo el grid y preguntarle a cada pieza sus rules de breakeo (quizás llamarle a una funcion que tiene)
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
            for (int i = 0; i < toDeleteList.Count; i++)
            {
                int roundedX = Mathf.RoundToInt(toDeleteList[i].transform.position.x);
                int roundedY = Mathf.RoundToInt(toDeleteList[i].transform.position.y);
                grid[roundedX, roundedY] = null;
                Destroy(toDeleteList[i].gameObject);

            }
            toDeleteList.Clear();
            StartCoroutine(WaitCoroutine());
            return true;
        }
        else { return false; }
    }

    void CheckForDowners()
    {
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
                    }

                }
            }
        }
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

    bool CompareColor(Transform grid1, Transform grid2, bool checkcolor)
    {
        if (!checkcolor)
        { return true; }
        else if ((grid1.gameObject.GetComponent<PieceBehavior>().color == grid2.gameObject.GetComponent<PieceBehavior>().color) ||
        (grid1.gameObject.GetComponent<PieceBehavior>().color == 99 || grid2.gameObject.GetComponent<PieceBehavior>().color == 99))
        { return true; }
        else return false;
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(2f);
    }
}