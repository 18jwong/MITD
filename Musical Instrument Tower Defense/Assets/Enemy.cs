using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int waypointIndex = 0;

    void Start() {

        // initialize target
        target = Waypoints.points[0];

    }

    void Update() {

        // find direction the enemy should travel in
        Vector3 dir = target.position - transform.position;
        // move enemy w/ speed * Time
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // if close enough to waypoint
        if (Vector3.Distance(transform.position, target.position) <= 0.4f) {

            // if enemy gets to final waypoint, delete
            if (waypointIndex >= Waypoints.points.Length - 1) {
                Destroy(gameObject);
                return;
            }

            // increment waypoint
            waypointIndex++;
            target = Waypoints.points[waypointIndex];

        }

    }

}
