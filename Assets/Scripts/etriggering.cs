using UnityEngine;

public class etriggering : MonoBehaviour
{
    private Transform player;
    private Animator animator;
    public float runRange = 15f;
    public float attackRange = 1.5f;
    private bool isAttacking = false;

    [Header("Agro Radius")]
    public SphereCollider agroRadiusCollider;
    public float agroRadius = 20f;

    private Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

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
        else
        {
            if (isAttacking)
            {
                StopAttack();
            }

            if (distanceToPlayer > attackRange && distanceToPlayer <= runRange)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                ChasePlayer();
            }
            else if (distanceToPlayer > runRange && distanceToPlayer <= agroRadius)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
                ChasePlayer();
            }
            else
            {
                ResetAnimatorParams();
            }
        }
    }

    void ChasePlayer()
    {
        if (isAttacking) return;

        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition);

        if (rb != null)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 3f));
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 3f);
        }
    }

    void AttackPlayer()
    {
        ResetAnimatorParams();
        isAttacking = true;
        animator.SetBool("isAttacking", true);

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }

    void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    private void ResetAnimatorParams()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isAttacking)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
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
}
