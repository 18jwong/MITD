using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeBtwnCheckGameEnds = 0.5f;
    public static bool GameIsOver;

    public GameObject gameOverUI;

    private void Start()
    {
        // When reloading the level, UI stays active so it must be disabled
        gameOverUI.SetActive(false);

        GameIsOver = false;

        StartCoroutine(CheckForGameEnd());
    }

    // CheckForGameEnd is called once every timeBtwnCheckGameEnds seconds
    IEnumerator CheckForGameEnd()
    {
        while (!GameIsOver)
        {
            // Temporary testing end of game
            if (Input.GetKeyDown("e"))
            {
                EndGame();
            }

            if (PlayerStats.Lives <= 0)
            {
                EndGame();
            }

            yield return new WaitForSeconds(timeBtwnCheckGameEnds);
        }
    }

    void EndGame()
    {
        GameIsOver = true;

        gameOverUI.SetActive(true);
    }
}
