using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Tower : MonoBehaviour
{
    [Header("Tower Properties")]
    public float health = 100;
    public TowerType towerType = TowerType.attacking;

    // ConditionalField( name of variable, 'not', the values to be true)
    // Attacking/buffing fields
    [ConditionalField("towerType", false, TowerType.attacking, TowerType.buffing)]
    public float range = 99f;
    [ConditionalField("towerType", false, TowerType.attacking, TowerType.buffing)]
    public TargetingMode targeting = TargetingMode.singleLane;

    // Money Generating fields
    [ConditionalField("towerType", false, TowerType.moneyGenerating)]
    public int dollarsPerSecond = 5;

    // These are passed into the projectile
    [Header("Projectile Properties")]
    [ConditionalField("towerType", false, TowerType.attacking)]
    public float speed = 30f;
    [ConditionalField("towerType", false, TowerType.attacking)]
    public float damage = 10f;
    [ConditionalField("towerType", false, TowerType.attacking)]
    public float shotsPerSecond = 1f;
    [ConditionalField("towerType", false, TowerType.attacking)]
    public float initialTimeUntilFire = 1f;
    private float fireCountdown;

    [ConditionalField("towerType", false, TowerType.attacking)]
    public GameObject projectilePrefab;
    [ConditionalField("towerType", false, TowerType.attacking)]
    public AnimationCurve speedVerticalCurve;

    // These are required for Unity to run
    [Header("Unity Setup Fields")]
    [ConditionalField("towerType", false, TowerType.attacking)]
    public Transform firePoint;
    public Animator animator;

    // Private variables
    private LinkedList<GameObject> enemiesToHit = new LinkedList<GameObject>();
    private int rowNum;
    private bool isDead = false;
    private bool isAttacking = false;

    private TowerManager towerManager;

    void Start()
    {
        towerManager = TowerManager.instance;

        // If tower is of type attacking, start checking attack animation
        if(towerType == TowerType.attacking)
        {
            // Projectile Setup
            fireCountdown = initialTimeUntilFire;

            InvokeRepeating("UpdateAnimation", 0f, 0.125f);

        } // If the tower is of type moneyGenerating, then don't start targetting
        else if (towerType == TowerType.moneyGenerating)
        {
            InvokeRepeating("GenerateMoney", 0f, 1f);
        }
    }

    void UpdateAnimation()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        LinkedListNode<GameObject> cursor = enemiesToHit.First;

        for (int i = 0; i < enemiesToHit.Count; i++)
        {
            GameObject e = cursor.Value;
            float distanceToEnemy = Vector2.Distance(transform.position, e.transform.position);
            bool enemyInFrontOfTower = transform.position.x < e.transform.position.x;

            if (distanceToEnemy < shortestDistance && enemyInFrontOfTower)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = e;
            }

            cursor = cursor.Next;
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            isAttacking = true;

            // Set animaton to attack
            animator.SetBool("Attacking", true);
        }
        else
        {
            isAttacking = false;
            // Set animaton to stop attacking
            animator.SetBool("Attacking", false);
        }
    }

    void GenerateMoney()
    {
        PlayerStats.AddMoney(dollarsPerSecond);
    }

    private void Update()
    {
        if (isDead)
            return;

        // If the tower is of type moneyGenerating, then don't update anything
        if (towerType == TowerType.moneyGenerating)
            return;

        fireCountdown -= Time.deltaTime;

        // If not attacking, do nothing
        if (!isAttacking)
        {
            return;
        }

        // Shoot() if countdown has been reset
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / shotsPerSecond;
        }
    }

    // Private helper functions ------------------------------

    private void Shoot()
    {
        // Creates bullet and casts it to type GameObject
        GameObject projectileGO = (GameObject)Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        // Calls Seek() in Bullet script
        if (targeting == TargetingMode.singleLane)
        {
            projectile.Seek(rowNum, speed, damage, speedVerticalCurve);
        }
        else if(targeting == TargetingMode.tripleLanes)
        {
            // Middle projectile
            projectile.Seek(rowNum, speed, damage, speedVerticalCurve);

            // Below projectile
            if(rowNum-1 >= 0)
            {
                projectileGO = (GameObject)Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                projectile = projectileGO.GetComponent<Projectile>();
                projectile.Seek(rowNum - 1, speed, damage, speedVerticalCurve);
            }

            // Above projectile
            if(rowNum+1 < towerManager.GetEnemiesList().Count)
            {
                projectileGO = (GameObject)Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                projectile = projectileGO.GetComponent<Projectile>();
                projectile.Seek(rowNum+1, speed, damage, speedVerticalCurve);
            }
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
    
    public LinkedList<GameObject> GetEnemiesList()
    {
        return enemiesToHit;
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
