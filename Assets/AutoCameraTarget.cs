using UnityEngine;
using Cinemachine;

public class AutoCameraTarget : MonoBehaviour
{
    private void Start()
    {
        // Pastikan karakter player diberi tag "Player"
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            var vcam = GetComponent<CinemachineVirtualCamera>();
            vcam.Follow = player.transform;
            vcam.LookAt = player.transform;
        }   
        else
        {
            Debug.LogWarning("Player not found. Make sure your player has the 'Player' tag.");
        }
    }
}
