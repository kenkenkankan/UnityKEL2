using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("Window Switcher")]
    [SerializeField] GameObject menu;

    public virtual void SwitchMenu()
    {
        if(menu != null) menu.SetActive(true);
        gameObject.SetActive(false);
    }
}
