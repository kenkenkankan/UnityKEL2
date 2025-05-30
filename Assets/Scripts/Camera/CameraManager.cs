using Cinemachine;
using UnityEngine;
using SmoothShakeFree;
using System;

public class CameraManager : MonoBehaviour
{

    public static event Action<bool> CameraChanged = delegate { };
    public CinemachineVirtualCamera mainVirtualCam;
    [SerializeField] CinemachineVirtualCamera activeVirtualCam; // May become obsolete

    [Header("Shaking Component")]
    [SerializeField] SmoothShake shakeComponent;
    public Vector2 shakeAmplitude;
    public bool isShaking;

    void Start()
    {
        activeVirtualCam = mainVirtualCam;
        shakeComponent = activeVirtualCam.GetComponentInParent<SmoothShake>();
        isShaking = false;
    }

    public void OnShake()
    {
        shakeComponent.rotationShake.noiseType = Shaker.NoiseType.WhiteNoise;
        shakeComponent.rotationShake.amplitude.x = shakeAmplitude.x;
        shakeComponent.rotationShake.amplitude.y = shakeAmplitude.y;
        shakeComponent.StartShake();
        isShaking = true;
    }

    public void StopShake()
    {
        shakeComponent.StopShake();
        isShaking = false;
    }

    void SwitchCamera(CinemachineVirtualCamera targetCam)
    {
        if (activeVirtualCam.Equals(targetCam)) return;

        activeVirtualCam.Priority = 0;
        targetCam.Priority = 1;
        activeVirtualCam = targetCam;

        shakeComponent = activeVirtualCam.GetComponentInParent<SmoothShake>();
    }

    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.CompareTag("Camera Zone"))
        {
            var newCam = trigger.GetComponent<CameraZoneSwitcher>().attachedCamera;
            SwitchCamera(newCam);
        }
    }
}
