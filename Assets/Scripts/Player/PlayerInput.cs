using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static event Action OnInteract = delegate { };

    [Header("Dialogue UI")]
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    [SerializeField] private Transform flashlightAnchor;

    const float BASE_MOVE_SPEED = 4;
    public bool isMoving;

    private Vector2 input;

    [Header("Layer Masks")]
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;

    private Animator animator;

    private Rigidbody rb;

    [Header("States Statuses")]
    PlayerState currentPlayerState;

    public PlayerState CurrentPlayerState
    {
        get => currentPlayerState;
        set
        {
            if (currentPlayerState == value || GameManager.Instance.currentState != GameManager.GameState.Gameplay) return;

            if (PlayerStats.Instance.isSanitySubsribed)
            {
                PlayerStats.OnSanityChanged -= (_) => PlayerStats.Instance.HandleSanityChange();
                PlayerStats.Instance.isSanitySubsribed = false;
            }

            currentPlayerState = value;

            if (currentPlayerState != PlayerState.NearGhost)
            {
                PlayerStats.OnSanityChanged += (_) => PlayerStats.Instance.HandleSanityChange();
                PlayerStats.Instance.isSanitySubsribed = true;
            }
        }
    }

    PlayerState previousPlayerState;

    public enum PlayerState
    {
        Idle, Moving, OnJournal, Interacting, OnDialogue, NearGhost
    }

    void OnEnable()
    {
        PlayerStats.OnSanityChanged += (_) => PlayerStats.Instance.HandleSanityChange();
        PlayerStats.Instance.isSanitySubsribed = true;
    }

    void OnDisable()
    {
        PlayerStats.OnSanityChanged -= (_) => PlayerStats.Instance.HandleSanityChange();;
        PlayerStats.Instance.isSanitySubsribed = false;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        dialogueUI = FindFirstObjectByType<DialogueUI>();
        flashlightAnchor = transform.GetChild(0);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (GameManager.Instance.currentState != GameManager.GameState.Gameplay) return;
        
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.E) && currentPlayerState == PlayerState.Idle)
            {
                if (Interactable != null)
                {
                    Interactable?.Interact(this);
                    Interactable?.ConfirmNotif.SetActive(false);
                    SetPlayerState(PlayerState.Interacting);
                }
            }

            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            && currentPlayerState == PlayerState.Idle)
            {
                SetPlayerState(PlayerState.Moving);
            }
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            SetPlayerState(PlayerState.Idle);
            input = Vector2.zero;
        }
        
        PlayerStateHandler();        
    }

    private void MovingController()
    {
        input.x = Input.GetAxisRaw("Horizontal");

        if (input.x != 0)
        {
            Vector3 scale = flashlightAnchor.localScale;
            scale.x = -Mathf.Sign(input.x); // flip arah X
            flashlightAnchor.localScale = scale;
        }

        if (input.x != 0) input.y = 0;

        if (input != Vector2.zero)
        {
            animator.SetFloat("MoveX", input.x);

            var targetPos = transform.position;
            targetPos.x += input.x;

            if (IsWalkable(targetPos))
                StartCoroutine(Move(targetPos));
        }

        animator.SetBool("IsMoving", isMoving);
    }


    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        SetPlayerState(PlayerState.Moving);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, BASE_MOVE_SPEED * PlayerStats.Instance.Stats.movementSpeed * Time.deltaTime);

            
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D));
                
        isMoving = false;

        SetPlayerState(PlayerState.Idle);
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }

        return true;
    }

    #region Playe State Handler Region
    // May or may not moved to Player Input
    public void SetPlayerState(PlayerState newState)
    {
        if (CurrentPlayerState == newState) return;

        CurrentPlayerState = newState;
    }

    // May or may not moved to Player Input
    void PlayerStateHandler()
    {
        //Depends on PlayerInput class
        switch (currentPlayerState)
        {
            case PlayerState.Idle:
                MovingController();
                break;
            case PlayerState.Moving:
                MovingController();
                break;
            case PlayerState.OnJournal:
                GameManager.Instance.SetGameState(GameManager.GameState.Paused);
                // Journal UI interactions, pause via key || ->move to game state?
                break;
            case PlayerState.Interacting:
            //For iteractable item
                GameManager.Instance.SetGameState(GameManager.GameState.Paused);
                // Intermediate state before dialogue & show monologue if can
                break;
            case PlayerState.OnDialogue:
            //for interactable npc
                GameManager.Instance.SetGameState(GameManager.GameState.Paused);
                // Dialogue UI interactions, pause via key
                break;
            case PlayerState.NearGhost:
                // Disable Recovery which configured in SanityStateHandler? work the same wih idle and walking
                break;
            default:
                Debug.LogWarning("Unknown Player State");
                break;
        }
    }

    #endregion
    // Making sure to unscribe event
    void OnDestroy() {
        PlayerStats.OnSanityChanged -= (_) => PlayerStats.Instance.HandleSanityChange();
    }
}
