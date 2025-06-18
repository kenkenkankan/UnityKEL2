using UnityEngine;

public class PlayerSanityManager : MonoBehaviour
{
    public enum SanityState { Fine, Damaged, Caution, Danger }
    public float maxSanity = 100f;
    public float currentSanity;

    public ParticleSystem fineEffect;
    public ParticleSystem damagedEffect;
    public ParticleSystem cautionEffect;
    public ParticleSystem dangerEffect;

    private SanityState currentState;

    private void Start()
    {
        currentSanity = maxSanity;
        UpdateSanityState();
    }

    private void Update()
    {
        UpdateSanityState();
    }

    public void ReduceSanity(float amount)
    {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
        UpdateSanityState();
    }

    void UpdateSanityState()
    {
        SanityState newState;

        if (currentSanity >= 75)
            newState = SanityState.Fine;
        else if (currentSanity >= 50)
            newState = SanityState.Damaged;
        else if (currentSanity >= 25)
            newState = SanityState.Caution;
        else
            newState = SanityState.Danger;

        if (newState != currentState)
        {
            currentState = newState;
            SwitchParticleEffect();
        }
    }

    void SwitchParticleEffect()
    {
        fineEffect.Stop();
        damagedEffect.Stop();
        cautionEffect.Stop();
        dangerEffect.Stop();

        switch (currentState)
        {
            case SanityState.Fine:
                fineEffect.Play(); break;
            case SanityState.Damaged:
                damagedEffect.Play(); break;
            case SanityState.Caution:
                cautionEffect.Play(); break;
            case SanityState.Danger:
                dangerEffect.Play(); break;
        }
    }
}
