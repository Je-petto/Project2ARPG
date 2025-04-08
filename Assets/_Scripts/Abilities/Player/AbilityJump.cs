using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityJump : Ability<AbilityJumpData>
{
    private bool isjumping = false;

    public AbilityJump(AbilityJumpData data, CharacterControl owner) : base(data,owner)
    {
        if (owner.Profile == null) return;


        data.jumpForce = owner.Profile.jumpforce;
        data.jumpDuration = owner.Profile.jumpduration;
    }

    public override void Activate()
    {
        if (owner.TryGetComponent<InputControl>(out var input))
        {
            input.actionInputs.Player.Jump.performed += InputJump;
        }
        
    }
    public override void Deactivate()
    {

        if (owner.TryGetComponent<InputControl>(out var input))
        {
            input.actionInputs.Player.Jump.performed -= InputJump;
        }
    }

    float elapsed;
    public override void FixedUpdate()
    {
        if (owner.rb == null || isjumping == false)
            return;
 
        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01( elapsed / data.jumpDuration );

        //owner.agent.Move(Vector3.up * height * Time.deltaTime);
        Vector3 velocity = ((CharacterControl)owner).rb.linearVelocity;
        velocity.y = data.jumpCurve.Evaluate(t) * data.jumpForce;
        ((CharacterControl)owner).rb.linearVelocity = velocity;

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
        owner.Animate(AnimatorHashes._JUMPUP, 0.1f);
    }
    private void JumpDown()
    {
        isjumping = false;
        owner.Animate(AnimatorHashes._JUMPDOWN, 0.02f);

    }

    private void InputJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            JumpUp();
    }


}
