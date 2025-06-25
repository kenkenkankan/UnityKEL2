using UnityEngine;

public class SanityUIManager : MonoBehaviour
{
    public GameObject fineGroup;
    public GameObject damagedGroup;
    public GameObject cautionGroup;
    public GameObject dangerGroup;

    public void UpdateVisual(float sanityValue)
    {
        fineGroup.SetActive(false);
        damagedGroup.SetActive(false);
        cautionGroup.SetActive(false);
        dangerGroup.SetActive(false);

        if (sanityValue >= 75)
            fineGroup.SetActive(true);
        else if (sanityValue >= 50)
            damagedGroup.SetActive(true);
        else if (sanityValue >= 25)
            cautionGroup.SetActive(true);
        else
            dangerGroup.SetActive(true);
    }
}
