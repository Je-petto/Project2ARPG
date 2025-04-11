using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "Abilities/Attack")]

public class AbilityAttackData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Attack;
    public override Ability CreateAbility(CharacterControl owner) =>  new AbilityAttack(this, owner);



    [Tooltip("공격 대상(target)")]
    [ReadOnly] public CharacterControl target;


}

