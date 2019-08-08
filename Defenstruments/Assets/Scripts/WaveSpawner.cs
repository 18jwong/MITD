using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;

    public TowerManager towerManager;

    public Grid grid;
    private SpawnPointHolder spawnPointHolder;
    private GameObject[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public float initialTimeBeforeStart = 2f;
    private float countdown = 2f;

    private int waveIndex = 0;
    private int enemiesAlive = 0;

    private void Start()
    {
        countdown = initialTimeBeforeStart;

        spawnPointHolder = grid.GetComponent<SpawnPointHolder>();
        spawnPoints = spawnPointHolder.spawnPoints;
    }

    // Update is called once per frame
    void Update()
    {
        // If enemies are still on screen, do nothing.
        if (enemiesAlive > 0)
            return;

        // If the last wave has been reached, end game.
        if (waveIndex >= waves.Length)
        {
            //gameManager.WinLevel(); // TODO: when all enemies die, end level
            this.enabled = false;
            return;
        }

        // If the given amount of time has reached zero, start the next wave and reset timer.
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];

        enemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        waveIndex++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        // Spawn enemy
        int randomPoint = (int)Mathf.Round(Random.Range(0, spawnPoints.Length));
        GameObject newEnemy = Instantiate(enemy, spawnPoints[randomPoint].transform.position, Quaternion.identity);

        // Set towerManager and waveSpawner reference for the enemy
        newEnemy.GetComponent<Enemy>().SetManagerReferences(towerManager, this);

        // Add the enemy to list of enemies towers should shoot at
        towerManager.AddEnemyToTowers(newEnemy, randomPoint);
    }

    public void DecrementEnemiesAlive()
    {
        enemiesAlive--;
    }
}
