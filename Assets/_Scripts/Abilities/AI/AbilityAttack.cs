
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class AbilityAttack : Ability<AbilityAttackData>
{

    // 현재 공격 모션 진행중인가 ?
    bool isAttacking = false;

    private CancellationTokenSource cts;
    
    public AbilityAttack(AbilityAttackData data, CharacterControl owner) : base(data,owner)
    {        
        if (owner.Profile == null) return;           
    }

    public override void Activate(object obj)
    {     
        
        cts?.Dispose();        
        cts = new CancellationTokenSource();
        
           
        //obj 는 공격 대상 (Target)
        data.target = obj as CharacterControl;
        if (data.target == null)
            return;

        owner.ui.Display(data.Flag.ToString());

        data.eventAttackBefore.Register(OneventAttackBefore);
    }

    public override void Deactivate()
    {        
        
        data.eventAttackBefore.Unregister(OneventAttackBefore);

        cts.Cancel();
        cts.Dispose();  
    }


    private void OneventAttackBefore(EventAttackBefore e)
    {   
        // 다른 Character Control 은 무시 ( 본인것만 처리 한다 )
        if (owner != e.from)
            return;

        // 가해자 , 피해자 정보를 담아서 Event 를 쏜다.
        data.eventAttackAfter.from = owner;
        data.eventAttackAfter.to = data.target;
        data.eventAttackAfter.damage = owner.State.damage;
        data.eventAttackAfter.Raise();
    }




    
    public override void Update()
    {
        if (isAttacking == true || data.target == null)
            return;

        CooltimeAsync().Forget();    
    
        owner.LookatY(data.target.eyepoint.position);
        AnimationClip aniclip = owner.Profile.ATTACK.Random();
        owner.Animate("ATTACK", owner.Profile.animatorOverride, aniclip, 0f, 0);
        owner.AnimateMovespeed(0f, true);
    }

    async UniTaskVoid CooltimeAsync()
    {
        try
        {
            isAttacking = true;
            await UniTask.WaitForSeconds(owner.Profile.attackspeed, cancellationToken: cts.Token);
            isAttacking = false;
        }  
        catch ( System.OperationCanceledException)      
        {
            //Debug.Log("쿨타임 취소");
        }
        catch ( System.Exception e )
        {
            Debug.LogException(e);
        }
    }

    
}

