using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Grid grid;
    public float timeBetweenChecks = 0.5f;
    public int dollarsPerSecond = 5;

    [Header("Unity Setup Fields")]
    public GameObject gameOverUI;
    public GameObject victoryUI;

    private bool gameIsOver = false;

    private TowerManager towerManager;

    // Start is called before the first frame update
    void Start()
    {
        if (gameOverUI == null)
            Debug.LogError("GameManager: no GameOverUI assigned");

        towerManager = TowerManager.instance;

        StartCoroutine(CheckGameOver());

        InvokeRepeating("MoneyAdder", 0f, 1f);
    }

    // Cycle through every enemy and check if they passed grid's x-coord.
    IEnumerator CheckGameOver()
    {
        while (!gameIsOver)
        {
            LinkedList<LinkedList<GameObject>> enemiesList = towerManager.GetEnemiesList();

            // Cycle through rows
            LinkedListNode<LinkedList<GameObject>> rowNode = enemiesList.First;
            for (int i = 0; i < enemiesList.Count; i++)
            {
                // Cycle through enemies in row
                LinkedList<GameObject> enemyNodeList = rowNode.Value;
                LinkedListNode<GameObject> enemyNode = enemyNodeList.First;

                for (int j = 0; j < enemyNodeList.Count; j++)
                {
                    GameObject enemyGO = enemyNode.Value;

                    if (enemyGO.transform.position.x < grid.transform.position.x)
                    {
                        LoseGame();
                    }

                    enemyNode = enemyNode.Next;
                }

                rowNode = rowNode.Next;
            }

            yield return new WaitForSeconds(timeBetweenChecks);
        }
    }

    private void LoseGame()
    {
        Debug.Log("GameOverManager: Game Lost, an enemy got by.");
        gameIsOver = true;
        gameOverUI.SetActive(true);
    }

    // Called by WaveSpawner
    public void WinLevel()
    {
        Debug.Log("GameOverManager: Game Won, all waves cleared.");
        gameIsOver = true;
        victoryUI.SetActive(true);
    }

    public bool GameIsOver()
    {
        return gameIsOver;
    }

    void MoneyAdder()
    {
        PlayerStats.AddMoney(dollarsPerSecond);
    }
}
