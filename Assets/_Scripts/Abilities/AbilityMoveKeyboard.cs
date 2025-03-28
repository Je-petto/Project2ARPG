using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityMoveKeyboard : Ability<AbilityMoveKeyboardData>
{

    private Transform cameraTransform;
    private Vector3 camFoward, camRight;
    private Vector3 direction;
    private float velocity;
    private int movespeed_hash;
    
    private InputAction.CallbackContext context;


    public AbilityMoveKeyboard(AbilityMoveKeyboardData data, CharacterControl owner) : base(data, owner)
    {
        cameraTransform = Camera.main.transform;
        velocity = data.rotatePerSec;

        movespeed_hash = Animator.StringToHash("MOVESPEED");

        if(movespeed_hash < 0f)
            Debug.LogError($"AbilityMoveKeyboard ] MOVESPEED 해시를 찾을 수 없음");

    }

    public override void Activate(InputAction.CallbackContext context)
    {
        this.context = context;

        // contest 가 canceled 는 => 키를 뗐다는 의미 => 도착했다.
        owner.isArrived = context.canceled;

    }

    private void InputKeyboard()
    {

        var axis = context.ReadValue<Vector2>();

        camFoward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camFoward.y = 0;
        camRight.y =0;

        camFoward.Normalize();
        camRight.Normalize();

        direction = (camFoward * axis.y + camRight * axis.x).normalized;
    }

    //물리연산용 업데이트
    public override void FixedUpdate()
    {

        InputKeyboard();
        Rotate();
        Movement();
    }

    void Movement()
    {
        //owner.agent.Move(direction * data.movePerSec * Time.deltaTime);
        //50을 곱한 이유 = movePerSec와 linearVelocity 값의 범위를 맞추기 위한 상수
        Vector3 movement = direction * data.movePerSec * 50f * Time.deltaTime;
        Vector3 velocity = new Vector3(movement.x, owner.rb.linearVelocity.y, movement.z);

        owner.rb.linearVelocity = velocity;

        if (owner.isGrounded ==true)
        {
            float v = Vector3.Distance(Vector3.zero, owner.rb.linearVelocity);
            float targetspeed = Mathf.Clamp01(v / data.movePerSec);
            float movespd = Mathf.Lerp(owner.animator.GetFloat(owner._MOVESPEED), targetspeed, Time.deltaTime * 18f);

            owner.animator?.SetFloat(owner._MOVESPEED, movespd);


        }


    }

    void Rotate()
    {
        if (direction == Vector3.zero)
            return;
        
        // Atan2의 역할 : Vector2(x,z) 가 있을 대 해당 각도를 알려준다 (radian)
        // pie(π, 3.14) = 180도, 2π = 360도
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float smoothangle = Mathf.SmoothDampAngle(owner.transform.eulerAngles.y, angle, ref velocity, 0.1f);
        owner.transform.rotation = Quaternion.Euler(0f, smoothangle, 0f );

    }
}
