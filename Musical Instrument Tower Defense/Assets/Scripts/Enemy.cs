using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;

    [HideInInspector]
    public float speed;

    public float health = 100;

    public int moneyValue = 50;

    public int livesValue = 1;

    public GameObject deathEffect;

    private void Start()
    {
        speed = startSpeed;
    }

    public void DecreaseHealth(float amount)
    {
        health -= amount;

        if (health <= 0) {
            Die();
        }
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);

    }

    void Die()
    {
        PlayerStats.AddMoney(moneyValue);

        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(gameObject);
    }

}
