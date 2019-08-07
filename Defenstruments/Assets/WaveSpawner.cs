using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;

    public TowerManager towerManager;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    public float initialTimeBeforeStart = 2f;
    private float countdown = 2f;

    private int waveIndex = 0;
    private static int enemiesAlive = 0;

    private void Start()
    {
        countdown = initialTimeBeforeStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesAlive > 0)
            return;

        if (waveIndex >= waves.Length)
        {
            //gameManager.WinLevel();
            this.enabled = false;
            return;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
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
        GameObject newEnemy = Instantiate(enemy, spawnPoint.position, Quaternion.identity);

        // Set towerManager reference for the enemy
        newEnemy.GetComponent<Enemy>().SetTMReference(towerManager);

        // Add the enemy to list of enemies towers should shoot at
        towerManager.AddEnemyToTowers(newEnemy);
    }

    public static void decrementEnemiesAlive()
    {
        enemiesAlive--;
    }
}
