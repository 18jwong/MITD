using UnityEngine;

public class Projectile : MonoBehaviour
{
    // These are controlled in the Tower script
    private float speed = 30f;
    private float damage = 10f;

    private Transform target;

    // Tower script should be calling this to set the values
    public void Seek(Transform t, float s, float d)
    {
        target = t;
        speed = s;
        damage = d;
    }

    void Update()
    {
        if(target == null)
        {
            Debug.Log("Projectile.cs: Enemy is null");
            Destroy(gameObject);
            return;
        }

        Vector2 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // DistanceThisFrame exceeds where target is, then HitTarget()
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        Damage(target);
        Destroy(gameObject);
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if(e != null)
        {
            e.DecreaseHealth(damage);
        }
    }
}
