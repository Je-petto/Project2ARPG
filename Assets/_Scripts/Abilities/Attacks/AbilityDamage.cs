
using UnityEngine;


public class AbilityDamage : Ability<AbilityDamageData>
{

    public AbilityDamage(AbilityDamageData data, CharacterControl owner) : base(data,owner)
    {        
        if (owner.Profile == null) return;   

        // 데미지 받을 수 있는 상태
        owner.isDamageable = true;

        owner.ui.Show(true);
        owner.ui.SetHealth(owner.State.health, owner.Profile.health);  
    }

    public override void Activate(object obj)
    {     
        // object : EventAttackAfter
        var e = obj as EventAttackAfter;
        if (e == null)
            return;


        // 타격 이펙트
        PoolManager.I.Spawn(e.particleHit, owner.eyepoint.position, Quaternion.identity, null);


        // 피격 이펙트 연출
        owner.feedback?.PlayImpact();


        // 타격 데미지 수치 이펙트
        Vector3 rndsphere = Random.insideUnitSphere;
        rndsphere.y = 0f;
        Vector3 rndpos = rndsphere * 0.5f + owner.eyepoint.position;
        var floating = PoolManager.I.Spawn(e.feedbackFloatingText, rndpos, Quaternion.identity, null) as PoolableFeedback;
        if (floating != null)
            floating.SetText($"{e.damage}");

        // 데미지 Slider UI 연출
        owner.State.health -= e.damage;
        owner.ui.SetHealth(owner.State.health, owner.Profile.health);      


        // 사망시 연출
        if (owner.State.health <= 0)
        {
            // 죽으면 본인정보를 이벤트로 전달
            data.eventDeath.target = owner;
            data.eventDeath.Raise();
        }
    }

    public override void Deactivate()
    {            
        // 데미지 받을 수 없는 상태
        owner.isDamageable = false;

        owner.ui.Show(false);
    }


    
}

