using UnityEngine;
using UnityEngine.AI;

public class RandomWalking : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public float range = 10.0f; // Rastgele hareket alan�

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // S�rekli y�r�me animasyonu ba�lat
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }

        // �lk hedefi belirle
        SetRandomDestination();
    }

    void Update()
    {
        // Yeni bir hedef belirleme (e�er hedefe ula��ld�ysa)
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetRandomDestination();
        }
    }

    void SetRandomDestination()
    {
        // Rastgele bir pozisyon belirle
        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, range, 1))
        {
            agent.SetDestination(hit.position);
        }
    }
}
