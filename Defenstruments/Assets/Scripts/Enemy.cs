using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Properties")]
    public float startSpeed = 10f;
    public float maxSpeedAdjustment = 1f;
    public float startHealth = 100f;
    public float damagePerHit = 10f;
    public float attacksPerSecond = 1f;
    public float range = 0.2f;
    public int moneyValue = 50;
    public int livesValue = 1;

    [Header("Unity Setup")]
    public Animator animator;

    // Changing variables (aka variables)
    private float speed;
    private float health;
    private bool attacking = false;
    private float attackCooldown = 0f;

    // Relatively static variables
    private bool isDead = false;
    private int rowNum;
    private LinkedList<GameObject> towersToHit = new LinkedList<GameObject>();
    private Transform target;

    private TowerManager towerManager;
    private WaveSpawner waveSpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        towerManager = TowerManager.instance;

        speed = startSpeed;
        health = startHealth;

        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        LinkedListNode<GameObject> cursor = towersToHit.First;

        for (int i = 0; i < towersToHit.Count; i++)
        {
            GameObject t = cursor.Value;
            float distanceToEnemy = Vector2.Distance(transform.position, t.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = t;
            }

            cursor = cursor.Next;
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            attacking = true;
            animator.SetBool("Attacking", attacking);
        }
        else
        {
            target = null;
            attacking = false;
            animator.SetBool("Attacking", attacking);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // In case it's not deleted yet.
        if (isDead)
            return;

        // If target is close enough to attack, attack.
        if (attacking) {
            // If the target dies before a new one is chosen, return.
            if (target == null)
                return;

            attackCooldown -= Time.deltaTime;

            // If the cooldown is still active, return.
            if (attackCooldown > 0f)
                return;

            // Reset cooldown
            attackCooldown = 1 / attacksPerSecond;

            // Subtracts health and doesn't allow enemy to move.
            target.GetComponent<Tower>().SubtractHealth(damagePerHit);
            return;
        }

        // Move enemy
        float speedAdjustment = speed + Random.Range(0, maxSpeedAdjustment);
        transform.Translate(-speedAdjustment * Time.deltaTime, 0, 0, Space.World);
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
        towerManager.RemoveEnemy(gameObject);
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

    public void AddTowerInList(GameObject tower)
    {
        towersToHit.AddLast(tower);
    }

    public void RemoveTowerInList(GameObject tower)
    {
        LinkedListNode<GameObject> tNode = towersToHit.First;
        for (int i = 0; i < towersToHit.Count; i++)
        {
            if (tNode.Value == tower)
            {
                towersToHit.Remove(tNode);
                return;
            }

            tNode = tNode.Next;
        }

        // If the enemy is not found, then we have an issue
        Debug.LogError("Enemy: tower not removed...\n\tRow's towers: " + towersToHit.ToString());
    }

}
