using UnityEngine;
using Cinemachine;

public class CameraFollowPlayer : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        }
    }
}
