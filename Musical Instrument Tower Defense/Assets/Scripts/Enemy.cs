﻿using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    public float startHealth = 100f;

    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float health;

    public int moneyValue = 50;

    public int livesValue = 1;

    public GameObject deathEffect;

    [Header("Unity Stuff")]
    public Image healthBar;

    private bool isDead = false;

    private void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }

    public void DecreaseHealth(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !isDead) {
            Die();
        }
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);

    }

    void Die()
    {
        isDead = true;

        PlayerStats.AddMoney(moneyValue);

        WaveSpawner.decrementEnemiesAlive();

        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(gameObject);
    }

}
