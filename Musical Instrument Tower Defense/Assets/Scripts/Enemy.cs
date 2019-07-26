using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;

    public int health = 100;

    public int moneyValue = 50;

    public int livesValue = 1;

    public GameObject deathEffect;

    private Transform target;
    private int waypointIndex = 0;

    void Start() {

        // initialize target
        target = Waypoints.points[0];

    }

    public void DecreaseHealth(int amount)
    {
        health -= amount;

        if (health <= 0) {
            Die();
        }
    }

    void Die()
    {
        PlayerStats.AddMoney(moneyValue);

        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(gameObject);
    }

    void Update() {

        // find direction the enemy should travel in
        Vector3 dir = target.position - transform.position;
        // move enemy w/ speed * Time
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // if close enough to waypoint
        if (Vector3.Distance(transform.position, target.position) <= 0.5f) {

            // if enemy gets to last waypoint
            if (waypointIndex >= Waypoints.points.Length - 1)
            {
                EndReached();
                return;
            }

            GetNextWaypoint();
        }

    }

    void EndReached()
    {
        PlayerStats.SubtractLives(livesValue);
        Destroy(gameObject);
    }

    void GetNextWaypoint() {
        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

}
