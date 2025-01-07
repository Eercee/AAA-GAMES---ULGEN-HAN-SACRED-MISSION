using System;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    private Transform player; 
    private Animator animator;
    public float runRange = 15f; 
    public float attackRange = 1.5f; 
    private bool isAttacking = false;

    [Header("Agro Radius")]
    public SphereCollider agroRadiusCollider; 
    public float agroRadius = 20f; 

    void Start()
    {
        
        animator = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        
        if (agroRadiusCollider == null)
        {
            agroRadiusCollider = gameObject.AddComponent<SphereCollider>();
            agroRadiusCollider.isTrigger = true; 
            agroRadiusCollider.radius = agroRadius; 
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                AttackPlayer();
            }
        }
        else if (distanceToPlayer > attackRange && distanceToPlayer <= runRange)
        {
            isAttacking = false;
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            ChasePlayer();
        }
        else if (distanceToPlayer > runRange && distanceToPlayer <= agroRadius)
        {
            isAttacking = false;
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
            ChasePlayer();
        }
        else
        {
            isAttacking = false;
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }
    }

    void ChasePlayer()
    {
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 3f);
    }

    void AttackPlayer()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
        //animator.SetTrigger("AttackTrigger");
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isAttacking)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); 
            }
        }
    }
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player algýlandý, kovalamaya baþla!");
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
