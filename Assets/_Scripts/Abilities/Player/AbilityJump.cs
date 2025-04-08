using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityJump : Ability<AbilityJumpData>
{
    private bool isjumping = false;

    private CharacterControl ownerCC;

    public AbilityJump(AbilityJumpData data, IActorControl owner) : base(data,owner)
    {
        if (owner.Profile == null) return;

        //미리 형변환해서 받아 놓는다
        ownerCC = ((CharacterControl)owner);

        data.jumpForce = owner.Profile.jumpforce;
        data.jumpDuration = owner.Profile.jumpduration;
    }

    public override void Activate()
    {

        ownerCC.actionInputs.Player.Jump.performed += InputJump;
        
    }
    public override void Deactivate()
    {


        ownerCC.actionInputs.Player.Jump.performed -= InputJump;
    }

    float elapsed;
    public override void FixedUpdate()
    {
        if (ownerCC.rb == null || isjumping == false)
            return;
 
        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01( elapsed / data.jumpDuration );

        //owner.agent.Move(Vector3.up * height * Time.deltaTime);
        Vector3 velocity = ((CharacterControl)owner).rb.linearVelocity;
        velocity.y = data.jumpCurve.Evaluate(t) * data.jumpForce;
        ((CharacterControl)owner).rb.linearVelocity = velocity;

        // 점프 시간 종료
        if (t > 0.3f && ownerCC.isGrounded)
            JumpDown();
            //owner.ability.Deactivate(data.Flag);

    }

    private void JumpUp()
    {
        if (ownerCC.rb == null || ownerCC.isGrounded == false)
            return;

        isjumping = true;
        elapsed = 0;
        ownerCC.Animate(AnimatorHashes._JUMPUP, 0.1f);
    }
    private void JumpDown()
    {
        isjumping = false;
        ownerCC.Animate(AnimatorHashes._JUMPDOWN, 0.02f);

    }

    private void InputJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            JumpUp();
    }


}
