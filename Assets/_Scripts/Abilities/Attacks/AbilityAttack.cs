using UnityEngine;
using UnityEngine.AI;

public class AbilityAttack : Ability<AbilityAttackData>
{

    private float attackspeed;

    public AbilityAttack(AbilityAttackData data, CharacterControl owner) : base(data,owner)
    {
        
        if (owner.Profile == null) return;

        attackspeed = owner.Profile.attackspeed;
    }

    public override void Activate(object obj)
    {
        //obj는 공격 대상 (Target)
        data.target = obj as CharacterControl;
        if (data.target == null)
            return;


        owner.Display(data.Flag.ToString());
        owner.AnimateOnceAttack(data.target.eyepoint.position);
        elapsed = 0;

    }
    
    public override void Deactivate()
    {
    }

    float elapsed;
    public override void Update()
    {
        if (data.target == null) return;

        elapsed += Time.deltaTime;
        if ( elapsed >= owner.Profile.attackspeed )
        {
            owner.AnimateOnceAttack(data.target.eyepoint.position);
            elapsed = 0f;
        }

        owner.AnimateMoveSpeed(0f);
    }


}

// EQS( Environment Query System)
// 은폐, 엄폐
// 가장 위협이 큰 오브젝트
// 안전한 지역