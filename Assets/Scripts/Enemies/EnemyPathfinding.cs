using UnityEngine;
using UnityEngine.AI;
public class EnemyPathfinding : MonoBehaviour
{
   
    private Transform player;
    [SerializeField] private float movementSpeed;

    private NavMeshAgent agent;


    private enum EnemyState
    {
        Idle, Patrol, Chasing, Returning
    }

    private EnemyState currentState;

    void Awake()
    {
        player = FindFirstObjectByType<PlayerStats>().transform;
    }

    private void Start() {
        // Pathfinding hantu
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = movementSpeed;
    }

    void Update()
    {
        // Buat hantu mengejar karakter
        agent.SetDestination(player.position);
    }
}
