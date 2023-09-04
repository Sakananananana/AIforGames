using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI buttonText;
    public GameObject character;
    Animator anim;

    private void Start()
    {
        anim = character.GetComponent<Animator>();
    }

    public void StartButton()
    {
        anim.SetBool("attack", true);
        title.gameObject.SetActive(false);
        buttonText.gameObject.SetActive(false);
        Invoke("LoadGame", 3f);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void LoadLevel(string levelName)
    { 
        SceneManager.LoadScene(levelName);
    }
}
