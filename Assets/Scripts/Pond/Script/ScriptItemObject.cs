using UnityEngine;

[CreateAssetMenu(fileName = "New Stick Item", menuName = "Inventory/StickItem")]
public class StickItemObject : ItemObject
{
    public int bonusAttempts = 3;

    public override void ApplyEffect(GameObject user)
    {
        PondManager pond = FindObjectOfType<PondManager>();
        if (pond != null && !pond.IsMiniGameActive())
        {
            pond.AddAttempts(bonusAttempts);
            Debug.Log($"Stick digunakan! Menambahkan {bonusAttempts} attempt ke Pond.");
        }
        else
        {
            Debug.Log("Stick hanya bisa digunakan sebelum memulai minigame Pond.");
        }
    }
}
