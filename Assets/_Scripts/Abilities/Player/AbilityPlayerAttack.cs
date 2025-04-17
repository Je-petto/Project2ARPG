
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityPlayerAttack : Ability<AbilityPlayerAttackData>
{
   

    public AbilityPlayerAttack(AbilityPlayerAttackData data, CharacterControl owner) : base(data,owner)
    {
        if (owner.Profile == null) return;        
    }

    public override void Activate(object obj)
    { 
        if (owner.TryGetComponent<InputControl>(out var input))
        {
            input.actionInput.Player.Attack.performed += InputAttack;
        }
    }

    public override void Deactivate()
    {
        if (owner.TryGetComponent<InputControl>(out var input))
        {
            input.actionInput.Player.Attack.performed -= InputAttack;
        }
    }


    // InputControl 있을때만 작동 ( Player 작동(O) , Enemy 는 작동 안함(X) )
    private void InputAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == false) return;
        
        AnimationClip aniclip = owner.Profile.ATTACK.Random();
        owner.AnimateTrigger("ATTACK", owner.Profile.animatorOverride, aniclip);
    }

}
