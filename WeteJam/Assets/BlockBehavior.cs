using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class BlockBehavior : MonoBehaviour
{
    public Vector3 rotationPoint;
    public bool isActivePiece = true;
    private float previousTime;
    public float fallTime = 0.4f;
    public float quickFallTime = 0.05f;
    public static int height = 12;
    public static int width = 8;
    public static Transform[,] grid = new Transform[width, height];
    private List<Transform> toDeleteList = new List<Transform>();   
    //public int pieceType; //0=caballo, 1=alfil ,2=torre, 3= dama
    public enum PieceType
    {
        Kinght,
        Bishop,
        Rook,
        King
    }
    public PieceType myPieceType;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isActivePiece)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                transform.position = transform.position + new Vector3(-1, 0, 0);
                if (!VaildMove())
                { transform.position -= new Vector3(-1, 0, 0); }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                transform.position = transform.position + new Vector3(1, 0, 0);
                if (!VaildMove())
                { transform.position -= new Vector3(1, 0, 0); }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Keypad0))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                if (!VaildMove())
                {transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);}
            }
        }

        if (Time.time - previousTime > ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) ? quickFallTime : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!VaildMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                do { 
                CheckForDowners();
                AddToGrid(transform);
                CheckForBreakers();
                 bool seguir=DeleteList();

                } while (seguir)



                this.enabled = false;
                FindObjectOfType<GameController>().SpawnNewBlock();
            }
            previousTime = Time.time;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Inactive")
        {
            isActivePiece = false;
        }
    }

    void AddToGrid( Transform blockTransform)
    {
        foreach (Transform children in blockTransform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            grid[roundedX, roundedY] = children;
            children.transform.rotation = Quaternion.identity; 
        }
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
                    { }
                    else if (pieceBehavior.type == 1)
                    {
                        if ((i + 1 < width && j + 1 < height))
                        {
                            if (grid[i + 1, j + 1] != null)
                            {
                                if (grid[i + 1, j + 1].gameObject.GetComponent<PieceBehavior>().type == 1)
                                {
                                    if  ((i + 2 < width && j + 2 < height))
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
                        if  ((i + 1 < width && j - 1 >= 0))
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
                    { }
                    else if (pieceBehavior.type == 3)
                    { }
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

    bool DeleteList()
    { 
        if(toDeleteList.Count>0)
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
            for (int j = 0; j < height-1; j++)
            {
                if (grid[i,j] == null)
                { }
            }
        }
    }

}