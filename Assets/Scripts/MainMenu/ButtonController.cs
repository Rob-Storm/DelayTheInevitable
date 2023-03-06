using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public GameObject main;

    public GameObject help;

    public AudioSource speaker;

    public AudioClip uiHover;

    public AudioClip uiClick;

    public AudioClip uiBack;

    private void Start()
    {

        help.SetActive(false);
        main.SetActive(true);
    }

    public void DayLevel()
    {
        SceneManager.LoadScene("DayLevel");
    }

    public void NightLevel()
    {
        SceneManager.LoadScene("NightLevel");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Help()
    {
        main.SetActive(false);
        help.SetActive(true);
    }

    public void Back()
    {
        help.SetActive(false);
        main.SetActive(true);
    }
}
