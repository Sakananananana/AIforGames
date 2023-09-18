using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI title;
    public GameObject settingPanel;
    public GameObject character;
    Animator anim;

    private void Start()
    {
        anim = character.GetComponent<Animator>();
    }

    public void LoadLevel(string levelName)
    {
        if (levelName == "Game_Scene")
        {
            Time.timeScale = 1.0f;
            title.gameObject.SetActive(false);
            buttonText.gameObject.SetActive(false);
            anim.SetBool("attack", true);
            StartCoroutine(LoadScene(levelName));
        }
        else 
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(levelName);
        }
    }

    IEnumerator LoadScene(string level)
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(level);
    }

    public void ToggleONSettingMenu()
    {
        settingPanel.SetActive(true);
    }

    public void ToggleOFFSettingMenu()
    { 
        settingPanel.SetActive(false);
    }
}
