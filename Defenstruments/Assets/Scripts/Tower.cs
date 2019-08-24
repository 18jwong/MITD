using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Properties")]
    public float range = 99f;
    public bool singleLaneTargetting = true;
    public float health = 100;

    // These are passed into the projectile
    [Header("Projectile Properties")]
    public float speed = 30f;
    public float damage = 10f;

    public GameObject projectilePrefab;
    public float shotsPerSecond = 1f;
    public float initialTimeUntilFire = 1f;
    private float fireCountdown;

    [Header("Unity Setup Fields")]
    public Transform firePoint;

    // Private variables
    private Transform target;
    private LinkedList<GameObject> enemiesToHit = new LinkedList<GameObject>();
    private int rowNum;
    private bool isDead = false;

    private TowerManager towerManager;

    void Start()
    {
        towerManager = TowerManager.instance;

        // Projectile Setup
        fireCountdown = initialTimeUntilFire;

        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        LinkedListNode<GameObject> cursor = enemiesToHit.First;

        for(int i = 0; i < enemiesToHit.Count; i++)
        {
            GameObject e = cursor.Value;
            float distanceToEnemy = Vector2.Distance(transform.position, e.transform.position);
            bool enemyInFrontOfTower = transform.position.x < e.transform.position.x;

            if(distanceToEnemy < shortestDistance && enemyInFrontOfTower)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = e;
            }

            cursor = cursor.Next;
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void Update()
    {
        if (isDead)
            return;

        fireCountdown -= Time.deltaTime;

        // If no target, do nothing
        if(target == null)
        {
            return;
        }

        // Shoot() if countdown has been reset
        if(fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / shotsPerSecond;
        }
    }

    // Private helper functions

    private void Shoot()
    {
        // Creates bullet and casts it to type GameObject
        GameObject projectileGO = (GameObject)Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        // Calls Seek() in Bullet script
        if (projectile != null)
        {
            projectile.Seek(target, speed, damage);
        }
    }

    // Manipulation Functions ------------------------------

    public void AddEnemyInList(GameObject enemy)
    {
        enemiesToHit.AddLast(enemy);
    }

    public void RemoveEnemyInList(GameObject enemy)
    {
        LinkedListNode<GameObject> eNode = enemiesToHit.First;
        for (int i = 0; i < enemiesToHit.Count; i++)
        {
            if (eNode.Value == enemy)
            {
                enemiesToHit.Remove(eNode);
                return;
            }

            eNode = eNode.Next;
        }

        // If the enemy is not found, then we have an issue
        Debug.LogError("Tower: enemy not removed...\n\tRow's enemies: " + enemiesToHit.ToString());
    }

    public bool ContainsEnemyInList(GameObject enemy)
    {
        return enemiesToHit.Contains(enemy);
    }

    public void SetRowNum(int rN)
    {
        rowNum = rN;
    }

    public int GetRowNum()
    {
        return rowNum;
    }

    public void SubtractHealth(float h)
    {
        health -= h;
        
        // If no health, dies
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    // Helper function for SubtractHealth
    private void Die()
    {
        isDead = true;

        // Remove the tower from targetting
        towerManager.RemoveTower(gameObject);

        Destroy(gameObject);
    }

    public float GetCurrentHealth()
    {
        return health;
    }
}
