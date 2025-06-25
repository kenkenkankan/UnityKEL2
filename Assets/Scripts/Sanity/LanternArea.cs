using UnityEngine;

public class LanternArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats.Instance.StartLanternRegen();
            Debug.Log("Player entered Lantern Area, starting regeneration.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats.Instance.StopLanternRegen();
        }
    }
}
