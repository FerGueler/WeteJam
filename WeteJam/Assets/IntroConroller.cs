using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IntroConroller : MonoBehaviour
{
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
        { Load1Player(); }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        { Load2Players(); }
    }

    public void Load2Players()
    { SceneManager.LoadScene(2); }

    public void Load1Player()
    { SceneManager.LoadScene(1); }
}
