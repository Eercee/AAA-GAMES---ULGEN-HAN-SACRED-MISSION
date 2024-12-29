using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public int damageAmount = 20;
    public LayerMask attackLayer;
    public Transform attackPoint;
    public float effectDelay = 0.5f;
    public AudioClip attackSound;
    //public ParticleSystem attackEffect;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        if (attackSound != null)
        {
            AudioSource.PlayClipAtPoint(attackSound, transform.position);
        }
        /*
        if (attackEffect != null)
        {
            attackEffect.Play();
        }
        */
        yield return new WaitForSeconds(effectDelay);

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, attackLayer);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                Debug.Log($"Hedefe {damageAmount} hasar verildi: {enemy.name}");
            }
        }

        if (hitEnemies.Length == 0)
        {
            Debug.Log("Saldýrý boþa gitti.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
