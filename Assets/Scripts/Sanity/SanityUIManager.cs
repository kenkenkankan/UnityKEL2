using System;
using UnityEngine;
using UnityEngine.UI;

public class SanityUIManager : MonoBehaviour
{
    [SerializeField] Gradient fineGrad;
    [SerializeField] Gradient damagedGrad;
    [SerializeField] Gradient cautionGrad;
    [SerializeField] Gradient dangerGrad;
    [SerializeField] ParticleSystem colorOver;
    [SerializeField] Color newColor;
    [SerializeField] bool debug;
    public void UpdateVisual(float sanityValue)
    {
        Debug.Log("invoke!");

        ParticleSystem.ColorOverLifetimeModule colormodule = colorOver.colorOverLifetime;
        if (sanityValue >= 75)
            colormodule.color = knob.color = newColor = fineGrad.colorKeys[0].color;
        else if (sanityValue < 75 && sanityValue >= 50)
            colormodule.color = knob.color = newColor = damagedGrad.colorKeys[0].color;
        else if (sanityValue < 50 && sanityValue >= 25)
            colormodule.color = knob.color = newColor = cautionGrad.colorKeys[0].color;
        else
            colormodule.color = knob.color = newColor = dangerGrad.colorKeys[0].color;
    }
    event Action<float> OnSanityChanged = delegate { };

    Image knob;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        knob = transform.GetChild(0).GetComponent<Image>();
        OnSanityChanged += (san) => UpdateVisual(san);
    }

    [Range(0, 100)] public float sanityVal;

    void Update()
    {
        animator.SetFloat("Sanity", sanityVal);
        OnSanityChanged?.Invoke(sanityVal);
    }
}
