using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // These are controlled in the Tower script
    private int rowNum;
    private Transform target;

    private float speed = 30f;
    private float damage = 10f;
    private AnimationCurve curve;

    private TowerManager towerManager;
    private SpawnPointHolder spawnPointHolder;

    private void Awake()
    {
        towerManager = TowerManager.instance;
        spawnPointHolder = SpawnPointHolder.instance;
    }

    // Tower script should be calling this to set the values
    public void Seek(int rN, float s, float d, AnimationCurve c)
    {
        rowNum = rN;
        speed = s;
        damage = d;
        curve = c;

        target = spawnPointHolder.spawnPoints[rowNum].transform;

        StartCoroutine(AdjustHeight());
    }

    void Update()
    {
        if (this.transform.position.x > target.position.x)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 dir = (target.position - transform.position);
        float distanceThisFrame = speed * Time.deltaTime;

        // if closest enemy is within DistanceThisFrame, then HitTarget()
        Transform closestEnemy = UpdateClosestEnemy(distanceThisFrame*2);
        if (closestEnemy != null)
        {
            HitEnemy(closestEnemy);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    // Helper functions -------------------------------------

    // Using the originating tower's enemy list, function finds closest enemy in range
    // else returns null if out of range
    private Transform UpdateClosestEnemy(float range)
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        LinkedList<LinkedList<GameObject>> enemiesList = towerManager.GetEnemiesList();
        LinkedList<GameObject> enemiesListRow = towerManager.NavigateToRow(enemiesList, rowNum);
        LinkedListNode<GameObject> cursor = enemiesListRow.First;

        for (int i = 0; i < enemiesListRow.Count; i++)
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
            return nearestEnemy.transform;
        }
        // Else
        return null;
    }

    // Fixes where bullet height should be for its row
    private IEnumerator AdjustHeight()
    {
        float t = 0f;

        while (t < curve.keys[curve.keys.Length-1].time)
        {
            t += Time.deltaTime;
            float percentage = curve.Evaluate(t);
            float difference = target.position.y - this.transform.position.y;

            // bullet is Below
            if (difference > 0)
            {
                this.transform.Translate(0, difference * percentage, 0, Space.World);
            } // bullet is Above
            else if (difference < 0)
            {
                this.transform.Translate(0, difference * percentage, 0, Space.World);
            }

            yield return 0;
        }
    }

    void HitEnemy(Transform enemy)
    {
        Damage(enemy);
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
