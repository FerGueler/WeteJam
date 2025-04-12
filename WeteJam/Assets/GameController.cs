using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    public GameObject block;
    private Vector3 spawnPosition = new Vector3 (2,12,0);
    private Vector3 nextBlockPosition = new Vector3(7, 10, 0);
    public GameObject nextBlock;
    private bool isPaused = false;
    public AudioClip rotateSound;
    public AudioClip destroySound;
    public AudioClip pauseSound;
    public AudioClip resumeSound;
    public int numberOfPlayers;
    public GameObject gameOverText;
    public GameObject gameOverText2;
    public GameObject pauseText;
    public GameObject shadeOver;
    public GameObject scoreTitleText;
    public GameObject levelUpText;
    public GameObject levelUpTitleText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboAnimationText;
    public TextMeshProUGUI scoredDisplay;
    public TextMeshProUGUI levelText;
    private float comboCountdown;
    private float scoredCountdown;
    private float levelUpCountdown;

    private void Awake()
    {
        numberOfPlayers = IntroConroller.numberOfPlayersSettings;
        BlockBehavior.score = 0;
        BlockBehavior.currentLevel = 0;
    }

    private void Start()
    {
        if (numberOfPlayers==2) 
        { 
            scoreText.gameObject.SetActive(false);
            scoreTitleText.SetActive(false);
            levelUpText.gameObject.SetActive(false);
            levelText.gameObject.SetActive(false);
            levelUpTitleText.SetActive(false);

        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton8)) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            TogglePause();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        scoreText.text = BlockBehavior.score.ToString();

        if (comboCountdown>0) 
        { comboCountdown -= Time.deltaTime; }
        else 
        { comboAnimationText.gameObject.SetActive(false); }

        if (scoredCountdown > 0)
        { scoredCountdown -= Time.deltaTime; }
        else
        { scoredDisplay.gameObject.SetActive(false); }

        
        if (levelUpCountdown > 0)
        { levelUpCountdown -= Time.deltaTime; }
        else
        { levelUpText.SetActive(false); }
    }

    void TogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            AudioSource.PlayClipAtPoint(resumeSound, new Vector3(3.5f, 5.5f, -10));
            pauseText.SetActive(false);
            shadeOver.SetActive(false);
        }
        else
        {
            AudioSource.PlayClipAtPoint(pauseSound, new Vector3(3.5f, 5.5f, -10));
            Time.timeScale = 0f;
            pauseText.SetActive(true);
            shadeOver.SetActive(true);
        }

        isPaused = !isPaused;
    }

    public void SpawnNewBlock(int playerToBelong)
    {
        if(CheckForGameOver())
        {
            gameOverText.SetActive(true);
            gameOverText2.SetActive(true);
            shadeOver.SetActive(true);
        }
        else {
        BlockBehavior.comboCount = 1;
        nextBlock.transform.position = spawnPosition;
        nextBlock.GetComponent<BlockBehavior>().enabled = true;
        if (numberOfPlayers > 1)
        {
            nextBlock.GetComponent<BlockBehavior>().playerBelongs = playerToBelong;

        }
        else
        {
            nextBlock.GetComponent<BlockBehavior>().playerBelongs = 0;
        }
        nextBlock = Instantiate(block, nextBlockPosition, Quaternion.identity);
        }
    }
    private bool CheckForGameOver()
    {
        bool gameOver = false;
        for (int i = 0; i < BlockBehavior.width; i++)
        {
            if (BlockBehavior.grid[i, BlockBehavior.height - 1] != null)
            {
                gameOver=true;
                return gameOver;
            }
        }
        return false;
    }


    public void ComboAnimation(int comboCountToDisplay)
    {
        comboAnimationText.gameObject.SetActive(true);
        comboAnimationText.text = "x" + comboCountToDisplay + " COMBO!";
        StartDeactivateComboCountdown();
    }

    public void UpdateScoredDisplay(int scored)
    {
        if (numberOfPlayers == 1) 
        { 
        scoredDisplay.gameObject.SetActive(true);
        scoredDisplay.text = "+" + scored;
        StartDeactivateScoredCountdown();
        }

    }

    public void DisplayLevelUp()
    {
        if (numberOfPlayers == 1)
        {
            levelText.text = BlockBehavior.currentLevel.ToString();
        levelUpText.gameObject.SetActive(true);
        StartDeactivateLevelUpCountdown();
        }
    }

    public void ActivateGameOverUI()
    { gameOverText.SetActive(true); }
    public void ActivatePauseUI()
    { pauseText.SetActive(true); }
    public void ActivateShadeOver()
    { shadeOver.SetActive(true); }

    public void StartDeactivateComboCountdown()
    {
        comboCountdown = 1;
    }

    public void StartDeactivateScoredCountdown()
    {
        scoredCountdown = 0.8f;
    }

    public void StartDeactivateLevelUpCountdown()
    {
        levelUpCountdown = 1.4f;
    }

    public void PlayRotateSound()
    {
        AudioSource.PlayClipAtPoint(rotateSound, new Vector3(3.5f, 5.5f, -10));
    }

    public void PlayDestroySound()
    {
        AudioSource.PlayClipAtPoint(destroySound, new Vector3(3.5f, 5.5f, -10));
    }
}




