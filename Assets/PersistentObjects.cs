using UnityEngine;

public class PersistentObjects : MonoBehaviour
{
    private static PersistentObjects instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
