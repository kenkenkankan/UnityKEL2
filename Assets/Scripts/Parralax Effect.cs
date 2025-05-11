using UnityEngine;

public class ParralaxEffect : MonoBehaviour
{
    float startPos;
    [Tooltip("Kecepatan gambar bergerak")]
    [SerializeField]float parralaxEffect;
    Camera cam;

    void Start()
    {
        startPos = transform.position.x;
        cam = Camera.main;
    }

    void Update()
    {
        float distance = cam.transform.position.x * parralaxEffect;
        transform.position = new Vector3 (startPos +distance, transform.position.y, transform.position.z);
    }
}
