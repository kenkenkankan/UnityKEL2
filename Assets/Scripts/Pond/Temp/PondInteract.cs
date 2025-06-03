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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player near pond");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left pond");
        }
    }
}
