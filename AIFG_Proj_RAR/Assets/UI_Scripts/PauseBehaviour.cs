using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBehaviour : UserInterface
{
    public bool paused = false;
    public GameObject pauseMenu;

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused == false) 
            {
                paused = true;
                
            }
            else if (paused == true)
            {
                paused = false;
            }   
        }

        SetPauseMenu(paused);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetPauseMenu(bool isPaused)
    { 
        paused = isPaused;

        Time.timeScale = (paused) ? 0 : 1;
        pauseMenu.SetActive(paused);
    }

    private void Start()
    {
        paused = false;
    }
}
