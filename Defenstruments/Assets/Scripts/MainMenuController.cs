using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public float disableTime = 5f;

    [Header("Unity Setup Fields")]
    public Animator mainMenuAnimator;
    public GameObject mainMenu;
    public Animator levelSelectAnimator;
    public GameObject levelSelect;

    private SceneFader sceneFader;

    // Start is called before the first frame update
    void Start()
    {
        sceneFader = SceneFader.instance;
    }

    public void ShowLevelSelect()
    {
        Debug.Log("MainMenuController: TODO: level select...");
        mainMenuAnimator.SetTrigger("FadingOut");
        StartCoroutine(DisableObject(mainMenu));
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

    IEnumerator DisableObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(disableTime);

        gameObject.SetActive(false);
    }
}
