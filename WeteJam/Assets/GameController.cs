using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject block;
    private Vector3 spawnPosition = new Vector3 (2,12,0);
    private Vector3 nextBlockPosition = new Vector3(7, 10, 0);
    public GameObject nextBlock;
    public void SpawnNewBlock()
    {
        nextBlock.transform.position = spawnPosition;
        nextBlock.GetComponent<BlockBehavior>().enabled= true;
        nextBlock=Instantiate(block, nextBlockPosition, Quaternion.identity);
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}



