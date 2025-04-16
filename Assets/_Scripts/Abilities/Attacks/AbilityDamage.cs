using UnityEngine;


public class AbilityDamage : Ability<AbilityDamageData>
{


    public AbilityDamage(AbilityDamageData data, CharacterControl owner) : base(data,owner)
    {
        if (owner.Profile == null) return;

        //데미지 받을 수 있는 상태
        owner.isDamagable = true;

        //ui 출현
        owner.ui.Show(true);
        //Health
        owner.ui.SetHealth(owner.Profile.health, owner.Profile.health);
    }

    public override void Activate(object obj)
    {
        // object : EventAttackAfter
        var e = obj as EventAttackAfter;
        if(e == null)
            return;




        // 타격 이펙트 
        PoolManager.I.Spawn(e.particleHit, owner.eyepoint.position, Quaternion.identity, null);

        //피격 이벤트 연출
        owner.feedback.PlayImpact();
      
        // 타격 데미지 수치 이펙트
        Vector3 rndsphere = Random.insideUnitSphere;
        rndsphere.y =0f;
        Vector3 rndpos = rndsphere * 0.5f + owner.eyepoint.position;
        var floating = PoolManager.I.Spawn(e.feedbackFloatingText, rndpos, Quaternion.identity, null) as PoolableFeedback;
        if(floating != null)
            floating.SetText($"{e.damage}");
        

        //데미지 SliderUI 연출
        owner.State.health -= e.damage;
        owner.ui.SetHealth(owner.State.health, owner.Profile.health);

        // 사망 시 연출
        if (owner.State.health <= 0f)
        {
            owner.Animate(AnimatorHashes._DEATH, 0.2f);
            owner.ability.RemoveAll();
        }


    }
    
    public override void Deactivate()
    {
        //데미지 받을 수 없는 상태
        owner.isDamagable = false;
        
        owner.ui.Show(false);
    }


}
