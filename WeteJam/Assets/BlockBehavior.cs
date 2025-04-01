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
    public static Transform[,] grid = new Transform[width, height];
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
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                if (!VaildMove())
                {transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);}
            }
        }

        if (Time.time - previousTime > ((Input.GetKey(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) ? quickFallTime : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!VaildMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckForBreakers();

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

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            grid[roundedX, roundedY] = children;
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
            //loopear por todo el grid y preguntarle a cada pieza sus rules de breakeo (quizás llamarle a una funcion que tiene)
        
        }




}