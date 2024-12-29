using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    public HealthBar healthBar;
    public Transform healthBarCanvas;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        if (healthBarCanvas != null)
        {
            PositionHealthBar();
        }
    }

    void Update()
    {
        if (healthBarCanvas != null)
        {
            healthBarCanvas.position = transform.position + new Vector3(0, 2, 0);
            healthBarCanvas.rotation = Camera.main.transform.rotation;
        }
    }

    void PositionHealthBar()
    {
        healthBarCanvas.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        healthBarCanvas.position = transform.position + new Vector3(0, 2, 0);
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " hasar aldý: " + damageAmount + ". Kalan can: " + currentHealth);

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " öldü!");
        /*
        if (GameMaster.Instance != null)
        {
            GameMaster.Instance.AddScore(10);
        }
        */
        Destroy(gameObject, 2f);
    }
}
