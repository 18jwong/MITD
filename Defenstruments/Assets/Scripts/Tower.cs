using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Properties")]
    public float range = 99f;
    public bool singleLaneTargetting = true;

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

    void Start()
    {
        fireCountdown = initialTimeUntilFire;

        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        // Checking in reverse order so when an enemy dies, the actual
        // first enemy will be removed.
        LinkedListNode<GameObject> cursor = enemiesToHit.Last;

        for(int i = 0; i < enemiesToHit.Count; i++)
        {
            GameObject e = cursor.Value;
            float distanceToEnemy = Vector2.Distance(transform.position, e.transform.position);

            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = e;
            }

            cursor = cursor.Previous;
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
        fireCountdown -= Time.deltaTime;

        if(target == null)
        {
            return;
        }

        if(fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / shotsPerSecond;
        }
    }

    // Manipulation Functions ------------------------------

    public void AddEnemyInList(GameObject enemy)
    {
        enemiesToHit.AddFirst(enemy);
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
        Debug.Log("Tower error: enemy not removed...\n\tRow's enemies: " + enemiesToHit.ToString());
    }

    public bool ContainsEnemyInList(GameObject enemy)
    {
        return enemiesToHit.Contains(enemy);
    }

    public void Shoot()
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

    public void SetRowNum(int rN)
    {
        rowNum = rN;
    }

    public int GetRowNum()
    {
        return rowNum;
    }
}
