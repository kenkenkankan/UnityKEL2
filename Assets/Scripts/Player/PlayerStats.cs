using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    public bool isSanitySubscribed { get; set; }
    public PlayerStats Stats => this;
    public float movementSpeed = 1f;


    [Header("Sanity Settings")]
    private float currentSanity = 100f; // Internal 0–1
    [SerializeField] private float baseSanity = 100f;
    [SerializeField] private float sanityRecoveryCooldown = 2f;

    [Header("Sanity Particles (UI Canvas)")]
    [SerializeField] private GameObject[] sanityParticles;

    [Header("Sanity Regeneration")]
    private Coroutine lanternRegenCoroutine;
    [SerializeField] private float lanternRegenRate = 0.01f;

    [Header("UI Text & Color")]
    [SerializeField] private TMP_Text sanityText;
    [SerializeField] private Gradient sanityGradient;

    private SanityState currentState;
    private CameraManager camManager;

    public enum SanityState
    {
        Stable,
        Stressed,
        Distressed,
        Unstable,
        Psychotic,
        Death
    }

    public enum VisualSanityState
    {
        Fine,
        Damaged,
        Caution,
        Danger
    }

    public static event Action<float> OnSanityChanged = delegate { };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        camManager = GetComponent<CameraManager>();
        currentSanity = 1f;
        UpdateSanityState();
        UpdateVisualEffectForSanity();
    }

    void OnEnable() => OnSanityChanged += HandleSanityChange;
    void OnDisable() => OnSanityChanged -= HandleSanityChange;

    public void ModifySanity(float amount)
    {
        currentSanity = Mathf.Clamp(currentSanity + amount, 0f, 100f);
        OnSanityChanged?.Invoke(currentSanity);
    }


    public void HandleSanityChange(float sanityValue)
    {
        UpdateSanityState();
        UpdateVisualEffectForSanity();

        StopAllCoroutines();
        StartCoroutine(RecoverSanity());
    }

    private IEnumerator RecoverSanity()
    {
        yield return new WaitForSeconds(sanityRecoveryCooldown);
        ModifySanity(baseSanity);
    }

    private void UpdateSanityState()
    {
        SanityState newState = GetSanityStateFromValue(currentSanity);
        if (currentState != newState)
        {
            currentState = newState;
            ApplyStateEffects(newState);
        }
    }

    private SanityState GetSanityStateFromValue(float sanity)
    {
        if (sanity <= 0.001f) return SanityState.Death;
        if (sanity < 0.25f) return SanityState.Psychotic;
        if (sanity < 0.45f) return SanityState.Unstable;
        if (sanity < 0.65f) return SanityState.Distressed;
        if (sanity < 0.85f) return SanityState.Stressed;
        return SanityState.Stable;
    }
    private VisualSanityState GetVisualSanityStateFromValue(float sanity)
    {
        if (sanity >= 0.75f) return VisualSanityState.Fine;
        if (sanity >= 0.5f) return VisualSanityState.Damaged;
        if (sanity >= 0.25f) return VisualSanityState.Caution;
        return VisualSanityState.Danger;
    }

    private void UpdateVisualEffectForSanity()
    {
        VisualSanityState visualState = GetVisualSanityStateFromValue(currentSanity);

        for (int i = 0; i < sanityParticles.Length; i++)
        {
            if (sanityParticles[i] != null)
            {
                // Cek apakah GameObject sudah dihancurkan
                if (sanityParticles[i] != null && !sanityParticles[i].Equals(null))
                {
                    sanityParticles[i].SetActive(i == (int)visualState);
                }
            }
        }
    }

    private void ApplyStateEffects(SanityState state)
    {
        switch (state)
        {
            case SanityState.Stable:
                baseSanity = 0.005f;
                camManager?.StopShake();
                break;
            case SanityState.Stressed:
                baseSanity = 0.004f;
                camManager?.StopShake();
                break;
            case SanityState.Distressed:
                baseSanity = 0.003f;
                camManager?.StopShake();
                break;
            case SanityState.Unstable:
                baseSanity = 0.002f;
                camManager?.SetShakeIntensity(new Vector2(0.3f, 0.3f));
                break;
            case SanityState.Psychotic:
                baseSanity = 0.001f;
                camManager?.SetShakeIntensity(new Vector2(0.5f, 0.5f));
                break;
            case SanityState.Death:
                camManager?.StopShake();
                GameManager.Instance?.SetGameState(GameManager.GameState.GameOver);
                break;
        }
    }

    public void StartLanternRegen()
    {
        if (lanternRegenCoroutine != null)
            StopCoroutine(lanternRegenCoroutine);

        lanternRegenCoroutine = StartCoroutine(LanternRegen());
    }

    public void StopLanternRegen()
    {
        if (lanternRegenCoroutine != null)
        {
            StopCoroutine(lanternRegenCoroutine);
            lanternRegenCoroutine = null;
        }
    }

    private IEnumerator LanternRegen()
    {
        while (true)
        {
            ModifySanity(lanternRegenRate);
            yield return new WaitForSeconds(0.5f); // sesuaikan frekuensi
        }
    }

    // Opsional untuk skrip lain
    public void ReduceSanity(float amount) => ModifySanity(-amount / 100f);
    public void RecoverSanity(float amount) => ModifySanity(amount / 100f);
}