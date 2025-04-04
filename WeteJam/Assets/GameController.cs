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
    private bool isPaused = false;

    public void SpawnNewBlock(int playerToBelong)
    {
        nextBlock.transform.position = spawnPosition;
        nextBlock.GetComponent<BlockBehavior>().enabled= true;
        nextBlock.GetComponent<BlockBehavior>().playerBelongs = playerToBelong;
        nextBlock =Instantiate(block, nextBlockPosition, Quaternion.identity);
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton8)) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (isPaused)
        {
            // Unpause the game
            Time.timeScale = 1f;
        }
        else
        {
            // Pause the game
            Time.timeScale = 0f;
        }

        isPaused = !isPaused;
    }
}




