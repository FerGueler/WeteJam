using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBehaviour : MonoBehaviour
{
    float fallTime = 1;
    public bool isActivePiece = true;
    public int pieceType; //0=alfil, 1=caballo ,2=torre 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivePiece)
        { 
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position = transform.position + new Vector3(-1,0,0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position = transform.position + new Vector3(1, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position = transform.position + new Vector3(0, -1, 0);
        }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag =="Inactive")
        {
            isActivePiece=false;
        }
    }




}
