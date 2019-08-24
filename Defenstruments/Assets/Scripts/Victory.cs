using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public string menuSceneName = "MainMenu";

    private SceneFader sceneFader;

    private void Start()
    {
        sceneFader = SceneFader.instance;
    }

    public void Retry()
    {
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        sceneFader.FadeTo(menuSceneName);
    }

    public void Next()
    {
        sceneFader.FadeToNext();
    }
}
