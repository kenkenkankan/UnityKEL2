using UnityEngine;

public class GhostAffectSanity : MonoBehaviour
{
    public float detectRadius = 5f;
    public float sanityDrainRate = 10f;

    private Transform player;
    private PlayerStats playerStats;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerStats = playerObj.GetComponent<PlayerStats>();
        }
    }

    private void Update()
    {
        if (player == null || playerStats == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRadius)
        {
            // Kurangi sanity berdasarkan waktu
            playerStats.ReduceSanity(sanityDrainRate * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
