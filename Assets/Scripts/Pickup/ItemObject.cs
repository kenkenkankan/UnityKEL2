using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemObject : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;

    public virtual void ApplyEffect(GameObject user)
    {
        Debug.Log($"{itemName} used, but it has no effect.");
    }
//}

//[CreateAssetMenu(fileName = "Stick", menuName = "Inventory/Stick Item")]
//public class StickItem : ItemObject
//{
//    public int extraAttempts = 3;

//    public override void ApplyEffect(GameObject user)
//    {
//        PondManager pond = GameObject.FindObjectOfType<PondManager>();
//        if (pond != null && pond.IsMiniGameActive())
//        {
//            pond.AddAttempts(extraAttempts);
//            Debug.Log("Stick berhasil digunakan!");
//        }
//        else
//        {
//            Debug.LogWarning("Minigame belum aktif atau PondManager tidak ditemukan.");
//        }
//    }
}
