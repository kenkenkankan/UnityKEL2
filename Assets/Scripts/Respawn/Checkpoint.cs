using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SphereCollider trigger;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            RespawnController.Instance.respawnPoint = transform;
            trigger.enabled = false;
            Debug.Log("Checkpoint reached: " + gameObject.name);
        }
    }
}