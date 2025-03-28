using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement : MonoBehaviour
{

    [SerializeField] float movespeed = 7f;
    [SerializeField] float rotatespeed = 10f;
    
    [SerializeField] float jumpForce = 1500f;
    [SerializeField] float gravityScale = 2f;



    private Rigidbody rb;
    private Transform cameraTransform;

    float horz, vert;
    Vector3 camForward, camRight;
    Vector3 movement;

    bool jump;
    public bool isGrounded;    



    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.1f);
        //Debug.DrawRay(transform.position + Vector3.up, Vector3.down , Color.red);

        CustomGravity();

        InputKeyboard();

        Jump();
        Rotate();
        Movement();
    }

    

    void InputKeyboard()
    {
        horz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
        jump = Input.GetButtonDown("Jump");

        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();
        
        movement = (camForward * vert + camRight * horz).normalized;
    }


    void Movement()
    {
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, movement * movespeed, Time.deltaTime * 10);
    }


    Quaternion targetRotation = Quaternion.identity;
    void Rotate()
    {
        if (movement != Vector3.zero)
            targetRotation = Quaternion.LookRotation(movement);

        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotatespeed * Time.deltaTime);
    }
    
    void Jump()
    {
        if (jump && isGrounded)
            rb.AddExplosionForce(jumpForce, transform.position, 5f);            
    }

    void CustomGravity()
    {
        rb.useGravity = isGrounded;

        if (!isGrounded)
            rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }
}