using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject block;
    private Vector3 spawnPosition = new Vector3 (3,12,0);
    public void SpawnNewBlock()
    {
        Instantiate(block, spawnPosition, Quaternion.identity);
    }

    /*
    PieceBehaviour[] allPieces;
    bool theresActivePiece = true;
    

    void Start()
    {
        
    }

   
    void Update()
    {
        allPieces = FindObjectsOfType<PieceBehaviour>();

        for (int i = 0; i < allPieces.Length; i++) 
        {
            if (allPieces[i].isActivePiece)
            { theresActivePiece = true;}
        }
        if (!theresActivePiece)
        { CheckForBreakers(); }
    }

    void CheckForBreakers()
    {
        
    }

    */
}
