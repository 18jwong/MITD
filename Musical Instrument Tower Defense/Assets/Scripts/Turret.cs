using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour { 

    private Transform target;

    [Header("General")]

    public float range = 15f;


    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public float initialTimeUntilFire = 1f;
    private float fireCountdown;

    [Header("Use Laser")]
    public bool useLaser = false;
    public LineRenderer lineRenderer;

    [Header("Unity Setup Fields")]

    public string enemyTag = "Enemy";

    public Transform partToRotate;
    public float turnSpeed = 10f;

    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        fireCountdown = initialTimeUntilFire;

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Creates an array of all GameObjects with tag "enemyTag" and cycles through to find closest
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
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

    // Update is called once per frame
    void Update()
    {
        if (!useLaser)
        {
            fireCountdown -= Time.deltaTime;
        }

        if (target == null) {
            // if turret is a laser, then when the target disappears the laser disappears
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                }
            }

            // if no target, then don't do anything
            return;
        }

        LockOnTarget();

        if (useLaser)
        {
            Laser();
        } else
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
        }

    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Laser()
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
    }

    void Shoot() {
        // Creates bullet and casts it to type GameObject
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        // Calls Seek() in Bullet script
        if (bullet != null) {
            bullet.Seek(target);
        }
    }

    // Display range when selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
