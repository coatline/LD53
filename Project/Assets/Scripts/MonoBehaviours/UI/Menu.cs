using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject help;
    [SerializeField] GameObject menu;

    public void ShowHelp()
    {
        help.SetActive(true);
        menu.SetActive(false);
    }

    public void ShowMenu()
    {
        help.SetActive(false);
        menu.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMenu()
    {
        Game.I.Restart();
        SceneManager.LoadScene("Menu");
    }
}
