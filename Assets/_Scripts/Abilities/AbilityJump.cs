using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityJump : Ability<AbilityJumpData>
{
    private bool isjumping = false;

    public AbilityJump(AbilityJumpData data, CharacterControl owner) : base(data,owner)
    {   
    }

    public override void Activate()
    {

        owner.actionInputs.Player.Jump.performed += InputJump;
        
    }
    public override void Deactivate()
    {


        owner.actionInputs.Player.Jump.performed -= InputJump;
    }

    float elapsed;
    public override void FixedUpdate()
    {
        if (owner.rb == null || isjumping == false)
            return;
 
        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01( elapsed / data.jumpDuration );

        //owner.agent.Move(Vector3.up * height * Time.deltaTime);
        Vector3 velocity = owner.rb.linearVelocity;
        velocity.y = data.jumpCurve.Evaluate(t) * data.jumpForce;
        owner.rb.linearVelocity = velocity;

        // 점프 시간 종료
        if (t > 0.3f && owner.isGrounded)
            JumpDown();
            //owner.ability.Deactivate(data.Flag);

    }

    private void JumpUp()
    {
        if (owner.rb == null || owner.isGrounded == false)
            return;

        isjumping = true;
        elapsed = 0;
        owner.animator?.CrossFadeInFixedTime(owner._JUMPUP, 0.1f, 0, 0f); //Base Layer = 0
    }
    private void JumpDown()
    {
            isjumping = false;
            owner.animator?.CrossFadeInFixedTime(owner._JUMPDOWN, 0.02f, 0, 0f);

    }

    private void InputJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            JumpUp();
    }


}
