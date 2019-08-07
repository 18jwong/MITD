using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 30f;
    private float damage = 10f;

    private Transform target;

    public void Seek(Transform t, float s, float d)
    {
        target = t;
        speed = s;
        damage = d;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            Debug.Log("Projectile.cs: Enemy is null");
            return;
        }

        Vector3 dir = target.position - transform.position;
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
