using UnityEngine;

public class PondInteraction : MonoBehaviour
{
    public GameObject pondUI; // Panel grid 5x5
    public float interactRange = 2f;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ActivatePondGrid();
        }
    }

    void ActivatePondGrid()
    {
        pondUI.SetActive(true);
        Time.timeScale = 0f; // Pause game jika perlu
        Debug.Log("Pond minigame activated");
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player near pond");
        }
    }

    void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left pond");
        }
    }
}
