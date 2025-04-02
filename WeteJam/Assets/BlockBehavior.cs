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
    public static int width = 8;
    public Sprite[] spriteList;
    public static Transform[,] grid = new Transform[width, height];
    private List<Transform> toDeleteList = new List<Transform>();
    public bool isInitialBlock;
    void Start()
    {
        foreach (Transform children in transform)
        {
            int r = Random.Range(0, 4);
            children.GetComponent<PieceBehavior>().type = r;
            children.GetComponent<SpriteRenderer>().sprite = spriteList[r];
        }
        if(!isInitialBlock) this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                transform.position = transform.position + new Vector3(-1, 0, 0);
                if (!VaildMove())
                { transform.position -= new Vector3(-1, 0, 0); }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                transform.position = transform.position + new Vector3(1, 0, 0);
                if (!VaildMove())
                { transform.position -= new Vector3(1, 0, 0); }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Joystick1Button3))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                if (!VaildMove())
                { transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90); }
            }
        

        if (Time.time - previousTime > ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Joystick1Button0)) ? quickFallTime : fallTime))
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
                                                AddToDeleteList(grid[i, j]);
                                                AddToDeleteList(grid[i + 1, j + 1]);
                                                AddToDeleteList(grid[i + 2, j + 2]);
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
                                            if (grid[i + 2,j].gameObject.GetComponent<PieceBehavior>().type == 2)
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
                                                AddToDeleteList(grid[i, j]);
                                                AddToDeleteList(grid[i + 1, j + 1]);
                                                AddToDeleteList(grid[i + 2, j + 2]);
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
                                                AddToDeleteList(grid[i, j]);
                                                AddToDeleteList(grid[i + 1, j - 1]);
                                                AddToDeleteList(grid[i + 2, j - 2]);
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
                                                AddToDeleteList(grid[i, j]);
                                                AddToDeleteList(grid[i + 1, j]);
                                                AddToDeleteList(grid[i + 2, j]);
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
        }
        //quizás esta función debería ser del GameController o algo, pero como tenemos es de q el grid es static, no se
        //loopear por todo el grid y preguntarle a cada pieza sus rules de breakeo (quizás llamarle a una funcion que tiene)
    }

    void AddToDeleteList(Transform toDeleteItem)
    {
        toDeleteList.Add(toDeleteItem);
    }

    void Check7KnightJumps (int x, int y, int x_prev, int y_prev)
    {
        if(-x!=1||-y!=2)
        {
            if ((x + x_prev + 1 < width && y + y_prev + 2 < height))
            {
                if (grid[x + x_prev + 1, y + y_prev + 2] != null)
                {
                    if (grid[x + x_prev + 1, y + y_prev + 2].gameObject.GetComponent<PieceBehavior>().type == 0)
                    {
                        AddToDeleteList(grid[x + x_prev, y + y_prev]);
                        AddToDeleteList(grid[x + x_prev + 1, y + y_prev + 2]);
                        AddToDeleteList(grid[x_prev, y_prev]);
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
                        AddToDeleteList(grid[x + x_prev, y + y_prev]);
                        AddToDeleteList(grid[x + x_prev + 2, y + y_prev + 1]);
                        AddToDeleteList(grid[x_prev, y_prev]);
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
                        AddToDeleteList(grid[x + x_prev, y + y_prev]);
                        AddToDeleteList(grid[x + x_prev + 2, y + y_prev + -1]);
                        AddToDeleteList(grid[x_prev, y_prev]);
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
                        AddToDeleteList(grid[x + x_prev, y + y_prev]);
                        AddToDeleteList(grid[x + x_prev + 1, y + y_prev + -2]);
                        AddToDeleteList(grid[x_prev, y_prev]);
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
                        AddToDeleteList(grid[x + x_prev, y + y_prev]);
                        AddToDeleteList(grid[x + x_prev + -1, y + y_prev + -2]);
                        AddToDeleteList(grid[x_prev, y_prev]);
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
                        AddToDeleteList(grid[x + x_prev, y + y_prev]);
                        AddToDeleteList(grid[x + x_prev + -2, y + y_prev + -1]);
                        AddToDeleteList(grid[x_prev, y_prev]);
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
                        AddToDeleteList(grid[x + x_prev, y + y_prev]);
                        AddToDeleteList(grid[x + x_prev + -1, y + y_prev + 2]);
                        AddToDeleteList(grid[x_prev, y_prev]);
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
                        AddToDeleteList(grid[x + x_prev, y + y_prev]);
                        AddToDeleteList(grid[x + x_prev + -2, y + y_prev + 1]);
                        AddToDeleteList(grid[x_prev, y_prev]);
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
            return true;
        }
        else { return false; }
    }

    void CheckForDowners()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 1; j < height - 1; j++)
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
}


