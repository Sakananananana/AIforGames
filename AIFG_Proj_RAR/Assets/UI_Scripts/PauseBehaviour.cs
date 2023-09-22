using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBehaviour : UserInterface
{
    PlayerMovements player;

    public bool paused;
    public bool isSetting;
    public GameObject pauseMenu;
    public GameObject settingMenu;
    public GameObject restartMenu;

    private void Start()
    {
        paused = false;
        isSetting = false;

        player = character.GetComponent<PlayerMovements>();
    }

    public void Update()
    {
        if (!player.isDead)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (paused == false)
                {
                    paused = true;
                    SetPauseMenu();

                }
                else if (paused == true)
                {
                    paused = false;
                    SetPauseMenu();
                }
            }
        }
        else
        { 
            RestartMenu();
        }

        if (AltarDamage.isDead)
        {
            RestartMenu(); 
        }
    }

    public void BackToPause()
    {
        pauseMenu.SetActive(true);
        settingMenu.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        AltarDamage.isDead = false;
        player.isDead = false;
        player.HP = 100;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetPauseMenu()
    { 
        Time.timeScale = (paused) ? 0 : 1;
        pauseMenu.SetActive(paused);
    }

    public void SettingMenu()
    {
        pauseMenu.SetActive(false);
        settingMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        paused = false;
        pauseMenu.SetActive(false);
    }

    public void RestartMenu()
    {
        Time.timeScale = 0;
        restartMenu.SetActive(true);
    }
}
