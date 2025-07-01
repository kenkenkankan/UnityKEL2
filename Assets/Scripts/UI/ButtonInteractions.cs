using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractions : MonoBehaviour
{
    [SerializeField] private Selectable button;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    [SerializeField] private Color textHover;
    [SerializeField] private Color textExit;
    [SerializeField] private Color textDisable;
    [SerializeField] private Color glowOrigin;

    private Material origin;
    private Material copy;

    void Start()
    {
        image = GetComponent<Image>();
        origin = text.fontSharedMaterial;
        copy = new(origin);
        text.fontSharedMaterial = copy;
    }

    public void Hover()
    {
        image.color = new Color(128, 128, 128, 125);
        text.color = textHover;
        copy.SetColor("_GlowColor", textHover);
        text.UpdateMeshPadding();
    }

    public void Exit()
    {
        image.color = new Color(128, 128, 128, 0);
        text.color = textExit;
        
        copy.SetColor("_GlowColor", glowOrigin);
        text.UpdateMeshPadding();
    }

    public void Select()
    {
        image.color = new Color(255, 255, 255, 255);
    }
}
