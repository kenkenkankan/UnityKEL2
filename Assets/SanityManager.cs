using UnityEngine;

public class SanityManager : MonoBehaviour
{
    [Range(0, 100)]
    public float sanity = 100f;

    public enum SanityState { Fine, Damaged, Caution, Danger }
    public SanityState currentState;

    [Header("Particle Effects")]
    public ParticleSystem fineEffect;
    public ParticleSystem damagedEffect;
    public ParticleSystem cautionEffect;
    public ParticleSystem dangerEffect;

    private void Update()
    {
        UpdateState();
    }

    void UpdateState()
    {
        SanityState newState;

        if (sanity >= 75)
            newState = SanityState.Fine;
        else if (sanity >= 50)
            newState = SanityState.Damaged;
        else if (sanity >= 25)
            newState = SanityState.Caution;
        else
            newState = SanityState.Danger;

        if (newState != currentState)
        {
            currentState = newState;
            UpdateEffect();
        }
    }

    void UpdateEffect()
    {
        // Stop all
        fineEffect.Stop();
        damagedEffect.Stop();
        cautionEffect.Stop();
        dangerEffect.Stop();

        // Play based on state
        switch (currentState)
        {
            case SanityState.Fine:
                fineEffect.Play();
                break;
            case SanityState.Damaged:
                damagedEffect.Play();
                break;
            case SanityState.Caution:
                cautionEffect.Play();
                break;
            case SanityState.Danger:
                dangerEffect.Play();
                break;
        }
    }

    // Public method to decrease sanity
    public void ReduceSanity(float amount)
    {
        sanity = Mathf.Clamp(sanity - amount, 0f, 100f);
    }

    // Public method to recover sanity
    public void RecoverSanity(float amount)
    {
        sanity = Mathf.Clamp(sanity + amount, 0f, 100f);
    }
}