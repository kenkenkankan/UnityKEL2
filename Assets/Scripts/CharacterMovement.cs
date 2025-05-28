using System.Collections;
using System.Globalization;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private Transform flashlightAnchor;

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
        }

        animator.SetBool("IsMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Interactable != null)
            {
                Interactable?.Interact(this);
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
        if(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }

        return true;
    }

}
