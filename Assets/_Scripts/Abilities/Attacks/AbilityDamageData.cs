using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "Abilities/Damage")]

public class AbilityDamageData : AbilityData
{
    #region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    //이벤트 받는 용도
    public EventAttackBefore eventAttackBefore;
    //이벤트 보내는 용도
    public EventAttackAfter eventAttackAfter;
    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion
    
    public override AbilityFlag Flag => AbilityFlag.Damage;
    public override Ability CreateAbility(CharacterControl owner) =>  new AbilityDamage(this, owner);



    [Tooltip("공격 대상(target)")]
    [ReadOnly] public CharacterControl target;



}

