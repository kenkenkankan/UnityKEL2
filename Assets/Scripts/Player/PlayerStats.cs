using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    public static PlayerStats Instance;

    public static event Action<float> OnSanityChanged = delegate { };
    [SerializeField]
    public bool isSanitySubsribed;

    public CharacterData.Stats Stats
    {
        get => currentStats;
        set
        {
            if (currentStats != value)
            {
                currentStats = value;
            }
            SanityStateHandler();
        }
    }

    [SerializeField] float currentSanityRecoveryRate;

    public enum SanityState
    {
        Stable, Stressed, Distressed, Unstable, Psychotic, Death
    }

    // PlayerState previousPlayerState;
    public SanityState currentSanityState;

    [Header("Sanity UI")]
    [SerializeField] TMP_Text sanityValueUI;
    [SerializeField] float baseSanityRecovery; // Recovery value set by current sanity value
    [SerializeField] Image sanityBar; // Temp, gonna change this into particleeffect
    [SerializeField] float SBarProgressTime; // Cooldown for each time the bar color will change
    float progressTarget; // next value 
    [SerializeField] Gradient sanityGradient;
    Color newColor;

    PlayerAnimations playerAnim;
    // CheckPoint lastCheckPoint
    CameraManager camManager;

    // 
    void OnEnable()
    {
        OnSanityChanged += (_) => HandleSanityChange();
        isSanitySubsribed = true;
    }

    void OnDisable()
    {
        OnSanityChanged -= (_) => HandleSanityChange();
        isSanitySubsribed = false;
    }

    void Awake()
    {
        // Singleton Instance check
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Extra is deleted");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        camManager = GetComponent<CameraManager>();
        playerAnim = GetComponent<PlayerAnimations>();

        PopulateSanityUI();


        InitState();
        sanityBar.color = sanityGradient.Evaluate(SBarProgressTime);
        OnSanityChanged = delegate { }; // Failsafe to empty the event
        isSanitySubsribed = false;

        UpdateSanityBar();
    }

    private void PopulateSanityUI()
    {
        GameObject go = GameObject.FindWithTag("Sanity UI");
        sanityBar = go.transform.GetChild(0).GetComponent<Image>();
        sanityValueUI = go.transform.GetChild(1).GetComponent<TMP_Text>();
    }

    private void InitState()
    {
        currentSanityState = SanityState.Stable;
    }

    public void HandleSanityChange()
    {
        StartCoroutine(RecoverSanity());
        UpdateSanityBar();
    }

    private void UpdateSanityBar()
    {
        progressTarget = Stats.Sanity / data.stats.Sanity;
        sanityValueUI.text = Mathf.Round(100 * Stats.Sanity).ToString();

        StartCoroutine(SanityBarSmoother());

        newColor = sanityGradient.Evaluate(progressTarget);
    }

    IEnumerator SanityBarSmoother()
    {
        float fillAmount = sanityBar.fillAmount;
        Color currentColor = sanityBar.color;

        float elapsedTime = 0;
        while (elapsedTime < SBarProgressTime)
        {
            elapsedTime += Time.deltaTime;
            sanityBar.fillAmount = Mathf.Lerp(fillAmount, progressTarget, elapsedTime / SBarProgressTime);

            sanityBar.color = Color.Lerp(currentColor, newColor, elapsedTime / SBarProgressTime);
            yield return null;
        }
    }

    void Update()
    {
        DecreaseSanity(); //temp
    }

    public void DecreaseSanity()
    {
        // CharacterData.Stats aaaaa = new ();
        // aaaaa.Sanity = -0.007f;
        // Stats += aaaaa;
    }

    IEnumerator RecoverSanity()
    {
        currentSanityRecoveryRate = baseSanityRecovery + Stats.sanityRecoveryRate;
        Stats.Sanity += currentSanityRecoveryRate;
        yield return new WaitForSeconds(2f); // Temp cooldown still to fast?
    }

    private void SetSanityState(SanityState newState)
    {
        if (currentSanityState == newState) return;

        currentSanityState = newState;
    }

    private void SanityStateHandler()
    {
        switch (Stats.Sanity)
        {
            //Temp values
            case >= 0.85f:
                SetSanityState(SanityState.Stable);
                baseSanityRecovery = 0.005f;
                break;
            case < 0.85f and >= 0.65f:
                SetSanityState(SanityState.Stressed);
                baseSanityRecovery = 0.004f;
                if (camManager.isShaking) camManager.StopShake();
                break;
            case < 0.65f and >= 0.45f:
                SetSanityState(SanityState.Distressed);
                baseSanityRecovery = 0.003f;
                if (camManager.isShaking) camManager.StopShake();
                break;
            case < 0.45f and >= 0.25f:
                SetSanityState(SanityState.Unstable);
                baseSanityRecovery = 0.002f;
                camManager.shakeAmplitude = new(0.3f, 0.3f);
                if (!camManager.isShaking) camManager.OnShake();
                break;
            case < 0.25f and > 0.001f:
                SetSanityState(SanityState.Psychotic);
                baseSanityRecovery = 0.001f;
                camManager.shakeAmplitude = new(0.5f, 0.5f);
                if (!camManager.isShaking) camManager.OnShake();
                break;
            case <= 0.001f:
                SetSanityState(SanityState.Death);
                camManager.StopShake();
                GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
                break;
            default:
                Debug.LogWarning("Unknown Sanity State");
                break;
        }
        OnSanityChanged?.Invoke(Stats.Sanity);
    }
}
