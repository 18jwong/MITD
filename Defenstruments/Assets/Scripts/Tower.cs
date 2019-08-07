using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Properties")]
    public float range = 99f;

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

    void Start()
    {
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

            if(distanceToEnemy < shortestDistance)
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

    public void AddEnemy(GameObject enemy)
    {
        enemiesToHit.AddLast(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemiesToHit.Remove(enemy);
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
}
