using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public class AbilityAttack : Ability<AbilityAttackData>
{

    // 현재 공격 모션 진행 중인가?
    bool isAttacking = false;
    private CancellationTokenSource cts;


    public AbilityAttack(AbilityAttackData data, CharacterControl owner) : base(data,owner)
    {
        
        if (owner.Profile == null) return;
    }

    public override void Activate(object obj)
    {
        //obj는 공격 대상 (Target)
        data.target = obj as CharacterControl;
        if (data.target == null)
            return;


        owner.Display(data.Flag.ToString());
    }
    
    public override void Deactivate()
    {

    }

    public override void Update()
    {
        if (isAttacking == true || data.target == null)
            return;

        CooltimeAsync().Forget();

        owner.LookatY(data.target.eyepoint.position);
        AnimationClip clip = owner.Profile.ATTACK.Random();
        float anispd = owner.Profile.attackspeed;
        owner.Animate("ATTACK", owner.Profile.animatorOverride, clip, anispd, 0.1f, 0);
        owner.AnimateMoveSpeed(0f, true);
    }

    
    async UniTaskVoid CooltimeAsync()
    {
        try
        {
            isAttacking = true;
            await UniTask.WaitForSeconds(owner.Profile.attackspeed); 
            isAttacking = false;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }

    }

}

// EQS( Environment Query System)
// 은폐, 엄폐
// 가장 위협이 큰 오브젝트
// 안전한 지역