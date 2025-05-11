using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed;
    public bool isMoving = false;
    
    Vector3 mousePos;
    Vector3 mouseWorldPos;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Movement();
            isMoving = true;
        }
    }

    void FixedUpdate()
    {
        if(isMoving)
            Move(mouseWorldPos);
    }

    void Movement()
    {
        rb.velocity = Vector3.zero;

        mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
    
        mouseWorldPos.y = mouseWorldPos.z = 0;
    }

    void Move(Vector3 targetPos)
    {
        Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        float compare = 2.7f;
        // Berhenti ketika sudah dekat dengan target
        // Value pembanding  tergantung size collider sepertinya
        if (Vector3.Distance(newPosition, targetPos) <= compare)
        {
            rb.velocity = Vector3.zero;
            isMoving = false;
        }
    }
}
