using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private SceneFader sceneFader;

    // Start is called before the first frame update
    void Start()
    {
        sceneFader = SceneFader.instance;
    }

    public void ShowLevelSelect()
    {
        Debug.Log("MainMenuController: TODO: level select...");
    }

    public void ShowOptions()
    {
        Debug.Log("MainMenuController: TODO: show options...");
    }

    public void QuitGame()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }
}
