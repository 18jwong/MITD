using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public float enableTime = 0.5f;
    public float disableTime = 2f;

    [Header("Unity Setup Fields")]
    public GameObject mainMenu;

    private GameObject currentMenu;

    private SceneFader sceneFader;

    // Start is called before the first frame update
    void Start()
    {
        currentMenu = mainMenu;

        sceneFader = SceneFader.instance;
    }

    public void ShowMenu(GameObject menu)
    {
        // Fade out currentMenu
        TransitionOut();

        // Fade in mainMenu
        StartCoroutine(EnableObject(menu));
        currentMenu = menu;
    }

    public void QuitGame()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }

    public void PickLevel(string str)
    {
        TransitionOut();

        sceneFader.FadeTo(str);
    }

    // private helper function for transitioning
    private void TransitionOut()
    {
        currentMenu.GetComponent<Animator>().SetTrigger("FadingOut");
        StartCoroutine(DisableObject(currentMenu));
    }

    IEnumerator EnableObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(enableTime);

        gameObject.SetActive(true);
    }

    IEnumerator DisableObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(disableTime);

        gameObject.SetActive(false);
    }
}
