
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityMoveKeyboard : Ability<AbilityMoveKeyboardData>
{
        
    private Transform cameraTransform;
    private Vector3 camForward, camRight;
    private Vector3 direction;
    


    public AbilityMoveKeyboard(AbilityMoveKeyboardData data, CharacterControl owner) : base(data, owner)
    {
        cameraTransform = Camera.main.transform;

        if (owner.Profile == null) return;

        data.movePerSec = owner.Profile.movespeed;
        data.rotatePerSec = owner.Profile.rotatespeed;        
    }

    public override void Activate(object obj)
    {
        if (owner.TryGetComponent<InputControl>(out var input))
        {
            // context 가 canceled 는 => 키 를 뗐다는 의미 => 도착했다
            input.actionInput.Player.Move.performed += InputMove;
            input.actionInput.Player.Move.canceled += InputStop;
        }
    }

    public override void Deactivate()
    {
        Stop();

        if (owner.TryGetComponent<InputControl>(out var input))
        {
            input.actionInput.Player.Move.performed -= InputMove;
            input.actionInput.Player.Move.canceled -= InputStop;
        }
    }

    private void InputMove(InputAction.CallbackContext ctx)
    {
        owner.isArrived = !ctx.performed;

        var axis = ctx.ReadValue<Vector2>();

        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();
        
        direction = (camForward * axis.y + camRight * axis.x).normalized;        
    }

    private void InputStop(InputAction.CallbackContext ctx)
    {
        owner.isArrived = ctx.canceled;

        if (ctx.canceled)
            Stop();
    }



    // 물리 연산용 Update
    public override void FixedUpdate()
    {
        Rotate();
        Movement();
    }


    void Stop()
    {
        direction = Vector3.zero;
        owner.rb.linearVelocity = Vector3.zero;
        owner.animator?.SetFloat("MOVESPEED", 0f);
    }    


    void Movement()
    {
        // 50 곱한 이유 : movePerSec 과 linearVelocity 값을 동기화 위한 상수
        Vector3 movement = direction * data.movePerSec * 50f * Time.deltaTime;
        Vector3 velocity = new Vector3(movement.x, owner.rb.linearVelocity.y, movement.z);

        owner.rb.linearVelocity = velocity;

        if (owner.isGrounded == true)
        {
            float v = Vector3.Distance(Vector3.zero,owner.rb.linearVelocity);
            float targetspeed = Mathf.Clamp01(v/data.movePerSec);
            float movespd = Mathf.Lerp(owner.animator.GetFloat("MOVESPEED"), targetspeed, Time.deltaTime * 10f );

            owner.animator?.SetFloat("MOVESPEED", movespd);
        }
    }


       
    void Rotate()
    {
        if (direction == Vector3.zero)
            return;
        
        // Atan2 역할 : Vector2(x,z) 가 있을때 해당 각도를 알려준다 (radian)
        // pie(π) (3.14) => 180 degree
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float smoothangle = Mathf.SmoothDampAngle(owner.transform.eulerAngles.y, angle, ref data.rotatePerSec, 0.1f );
        owner.transform.rotation = Quaternion.Euler(0f, smoothangle, 0f);
    }
}
