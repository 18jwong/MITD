using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeBtwnCheckGameEnds = 0.5f;

    private bool gameEnded = false;

    private void Start()
    {
        StartCoroutine(CheckForGameEnd());
    }

    // CheckForGameEnd is called once every timeBtwnCheckGameEnds seconds
    IEnumerator CheckForGameEnd()
    {
        while (!gameEnded)
        {
            if (PlayerStats.Lives <= 0)
            {
                EndGame();
            }

            yield return new WaitForSeconds(timeBtwnCheckGameEnds);
        }
    }

    void EndGame()
    {
        gameEnded = true;
        Debug.Log("Game Over!");
    }
}
