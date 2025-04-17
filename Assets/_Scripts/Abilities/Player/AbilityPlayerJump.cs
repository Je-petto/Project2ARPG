using Project2ARPG;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityPlayerJump : Ability<AbilityPlayerJumpData>
{

    private bool isjumping = false;

    

    public AbilityPlayerJump(AbilityPlayerJumpData data, CharacterControl owner) : base(data,owner)
    {
        if (owner.Profile == null) return;
        
        data.jumpForce = owner.Profile.jumpforce;
        data.jumpDuration = owner.Profile.jumpduration;
    }

    public override void Activate(object obj)
    { 
        if (owner.TryGetComponent<InputControl>(out var input))
        {
            input.actionInput.Player.Jump.performed += InputJump;
        }
    }

    public override void Deactivate()
    {
        if (owner.TryGetComponent<InputControl>(out var input))
        {
            input.actionInput.Player.Jump.performed -= InputJump;
        }
    }

    float elapsed;
    public override void FixedUpdate()
    {
        if (owner.rb == null || isjumping == false)
            return;

        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01(elapsed / data.jumpDuration);
        
        Vector3 velocity = owner.rb.linearVelocity;
        velocity.y = data.jumpCurve.Evaluate(t) * data.jumpForce;
        owner.rb.linearVelocity = velocity;

        if (t > 0.3f && owner.isGrounded)
            JumpDown();
    }

    private void JumpUp()
    {
        if (owner.rb == null || owner.isGrounded == false)
            return;

        isjumping = true;
        elapsed = 0;
        
        owner.Animate("JUMPUP", 0.1f);
    }

    private void JumpDown()
    {
        isjumping = false;
        owner.Animate("JUMPDOWN", 0.02f);     
    }

    private void InputJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            JumpUp();
    }
}
