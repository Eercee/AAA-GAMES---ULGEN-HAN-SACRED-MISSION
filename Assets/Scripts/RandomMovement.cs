using UnityEngine;
using UnityEngine.AI;

public class RandomWalking : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public float range = 10.0f; // Rastgele hareket alaný

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Sürekli yürüme animasyonu baþlat
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }

        // Ýlk hedefi belirle
        SetRandomDestination();
    }

    void Update()
    {
        // Yeni bir hedef belirleme (eðer hedefe ulaþýldýysa)
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
