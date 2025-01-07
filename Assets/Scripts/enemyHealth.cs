using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;
    private Animator animator;

    public HealthBar healthBar;
    public Transform healthBarCanvas;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator bileþeni atanmadý!");
        }

        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }

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

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        Destroy(gameObject, 2f);
    }
}
