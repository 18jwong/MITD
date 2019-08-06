using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Properties")]
    public float startSpeed = 10f;
    public float startHealth = 100f;
    public int moneyValue = 50;
    public int livesValue = 1;

    private float speed;
    private float health;

    private bool isDead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        transform.Translate(-speed * Time.deltaTime, 0, 0);
    }

    // Manipulation Functions ------------------------------
    
    public void DecreaseHealth(float amount)
    {
        health -= amount;

        if(health <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        Destroy(gameObject);
    }
}
