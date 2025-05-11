using UnityEngine;
using UnityEngine.AI;
public class EnemyPathfinding : MonoBehaviour
{
   
    Transform player;
    [SerializeField] float movementSpeed;

    NavMeshAgent agent;

    void Awake()
    {
        player = FindFirstObjectByType<CharacterMovement>().transform;
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
