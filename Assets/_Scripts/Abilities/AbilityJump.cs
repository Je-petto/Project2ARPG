using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityJump : Ability<AbilityJumpData>
{
    private bool isjumping = false;

    public AbilityJump(AbilityJumpData data, CharacterControl owner) : base(data,owner)
    {   
    }

    public override void Activate(InputAction.CallbackContext ctx)
    {
        if (owner.rb == null || owner.isGrounded == false)
            return;

        isjumping = true;
        elapsed = 0;

        //owner.animator?.SetTrigger("jumpUp");

        owner.animator?.CrossFadeInFixedTime(owner._JUMPUP, 0.1f, 0, 0f); //Base Layer = 0
        
    }
    public override void Deactivate()
    {
        isjumping = false;

        owner.animator?.CrossFadeInFixedTime(owner._JUMPDOWN, 0.02f, 0, 0f); 
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
            owner.ability.Deactivate(data.Flag);

    }

}
