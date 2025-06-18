using UnityEngine;

public class SanitySystem : MonoBehaviour
{
    public float maxSanity = 100f;
    public float currentSanity;
    public float sanityDecreaseRate = 5f; // per second
    public float ghostDetectRange = 5f;

    public Transform ghost;
    public Transform player;

    void Start()
    {
        currentSanity = maxSanity;
    }

    void Update()
    {
        float distance = Vector2.Distance(player.position, ghost.position);

        if (distance <= ghostDetectRange)
        {
            currentSanity -= sanityDecreaseRate * Time.deltaTime;
            currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
        }

        if (currentSanity <= 0)
        {
            // Trigger kematian
            Debug.Log("Player mati karena kehilangan kewarasan.");
        }
    }
}
