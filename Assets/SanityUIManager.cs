using UnityEngine;

public class SanityUIManager : MonoBehaviour
{
    public GameObject fineGroup;
    public GameObject damagedGroup;
    public GameObject cautionGroup;
    public GameObject dangerGroup;

    public float maxSanity = 100f;
    public float currentSanity;

    void Start()
    {
        currentSanity = maxSanity;
        UpdateUI();
    }

    public void ReduceSanity(float amount)
    {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
        UpdateUI();
    }

    void UpdateUI()
    {
        fineGroup.SetActive(false);
        damagedGroup.SetActive(false);
        cautionGroup.SetActive(false);
        dangerGroup.SetActive(false);

        if (currentSanity >= 75)
        fineGroup.SetActive(true);
        else if (currentSanity >= 50)
            damagedGroup.SetActive(true);
        else if (currentSanity >= 25)
            cautionGroup.SetActive(true);
        else
            dangerGroup.SetActive(true);
    }
}
