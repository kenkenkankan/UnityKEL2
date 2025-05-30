using UnityEngine;

public interface IInteractable
{
    Vector2 InitPosition { get; }
    /// <summary>
    /// Interact notification popup canvas
    /// </summary>
    GameObject ConfirmNotif { get; }
    void Interact(PlayerInput player = null);
}