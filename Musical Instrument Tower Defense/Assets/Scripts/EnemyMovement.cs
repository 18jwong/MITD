using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int waypointIndex = 0;

    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();

        // initialize target
        target = Waypoints.points[0];

    }

    void Update()
    {
        // find direction the enemy should travel in
        Vector3 dir = target.position - transform.position;
        // move enemy w/ speed * Time
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        // if close enough to waypoint
        if (Vector3.Distance(transform.position, target.position) <= 0.5f)
        {
            GetNextWaypoint();
        }

        enemy.speed = enemy.startSpeed;
    }

    void GetNextWaypoint()
    {
        // if enemy gets to last waypoint
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            EndReached();
            return;
        }

        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    void EndReached()
    {
        PlayerStats.SubtractLives(enemy.livesValue);
        Destroy(gameObject);
    }
}
