using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Properties")]
    public float range = 99f;
    public float damage = 10f;

    public GameObject projectilePrefab;
    public float shotsPerSecond = 1f;
    public float initialTimeUntilFire = 1f;
    private float fireCountdown;

    [Header("Unity Setup Fields")]
    public Transform firePoint;

    // Private variables
    private Transform target;
    private Enemy targetEnemy;
    private Enemy[] enemiesInLane;

    // Start is called before the first frame update
    void Start()
    {
        fireCountdown = initialTimeUntilFire;

        //InvokeRepeating()
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Manipulation Functions ------------------------------

    public void AddEnemy(Enemy enemy)
    {

    }
}
