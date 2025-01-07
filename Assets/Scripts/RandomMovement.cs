using UnityEngine;
using UnityEngine.AI;

public class RandomWalking : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public float range = 10.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }

        SetRandomDestination();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetRandomDestination();
        }

        UpdateAnimation();
        RotateTowardsMovementDirection();
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, range, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    void RotateTowardsMovementDirection()
    {
        if (agent.velocity.sqrMagnitude > 0.01f) 
        {
            Vector3 direction = agent.velocity.normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); 
        }
    }
}
