using System.Collections;
using System.Globalization;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable { get; set; }

    public float moveSpeed;
    public bool isMoving;
    private Vector2 input;

    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;

    private Animator animator;

    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (dialogueUI.IsOpen) return;

        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            animator.SetFloat("MoveX", input.x);

            if (input != Vector2.zero)
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }
        }
    }



    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }

        return true;
    }

}
