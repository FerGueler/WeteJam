using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IntroConroller : MonoBehaviour
{
    public static int numberOfPlayersSettings;
    public TextMeshProUGUI highscoreText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Load1Player();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Load2Player();
        }

        highscoreText.text = PlayerPrefs.GetInt("recordScore").ToString();
    }

    public void Load1Player()
    {
        numberOfPlayersSettings = 1;
        SceneManager.LoadScene(1);

    }
    public void Load2Player()
    {
        numberOfPlayersSettings = 2;
        SceneManager.LoadScene(1);

    }
}
