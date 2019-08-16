using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Properties")]
    public float startSpeed = 10f;
    public float startHealth = 100f;
    public int moneyValue = 50;
    public int livesValue = 1;

    private float speed;
    private float health;

    private int rowNum;
    private bool isDead = false;

    private TowerManager towerManager;
    private WaveSpawner waveSpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        towerManager = TowerManager.instance;

        speed = startSpeed;
        health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        transform.Translate(-speed * Time.deltaTime, 0, 0);
    }

    // Manipulation Functions ------------------------------
    
    public void DecreaseHealth(float amount)
    {
        health -= amount;

        if(health <= 0 && !isDead)
        {
            Die();
        }
    }

    // Helper function for DecreaseHealth()
    private void Die()
    {
        isDead = true;

        // Remove the enemy from targetting and wave spawner
        towerManager.RemoveEnemy(gameObject, rowNum);
        waveSpawner.DecrementEnemiesAlive();

        // Add money
        PlayerStats.AddMoney(moneyValue);

        Destroy(gameObject);
    }

    public void SetManagerReferences(WaveSpawner ws)
    {
        waveSpawner = ws;
    }

    public void SetRowNum(int rN)
    {
        rowNum = rN;
    }

    public int GetRowNum()
    {
        return rowNum;
    }
}
