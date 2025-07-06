using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Obsolete("Ganti Ke Animasi aja")]
public class ButtonInteractions : MonoBehaviour
{
    [SerializeField] private Selectable button;
    [SerializeField] private Image image;
    [SerializeField] private List<TMP_Text> texts;
    [SerializeField] private Color textHover;
    [SerializeField] private Color textExit;
    [SerializeField] private Color textDisable;
    [SerializeField] private Color glowOrigin;
    
    [SerializeField] private Material origin;
    private Material copy;
    private bool HasMaterial = false;

    void Start()
    {
        if (origin != null)
        {
            origin = texts[0].fontSharedMaterial;
            copy = new(origin);
            HasMaterial = true;
            texts[0].fontSharedMaterial = copy;
        }
    }

    public void Hover()
    {
        image.color = new Color(128, 128, 128, 125);
        SetFontGlow(textHover);
        foreach (var text in texts)
        {
            text.color = textHover;
            text.UpdateMeshPadding();
        }
    }

    public void Exit()
    {
        image.color = new Color(128, 128, 128, 0);

        SetFontGlow(glowOrigin);
        foreach (var text in texts)
        {
            text.color = textExit;
            text.UpdateMeshPadding();
        }
    }

    private void SetFontGlow(Color color)
    {
        if(HasMaterial)
            copy.SetColor("_GlowColor", glowOrigin);
    }

    public void Select()
    {
        image.color = new Color(255, 255, 255, 255);
    }

    [ContextMenu("GetAllTextInChildren")]
    private void GetAllTextInChildren()
    {
        texts = GetComponentsInChildren<TMP_Text>().ToList();
    }
}
