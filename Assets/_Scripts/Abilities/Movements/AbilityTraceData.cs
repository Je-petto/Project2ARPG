using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "Abilities/Trace")]

public class AbilityTraceData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Trace;
    public override Ability CreateAbility(CharacterControl owner) =>  new AbilityTrace(this, owner);

    [ReadOnly] public float movePerSec = 5f;
    [ReadOnly] public float rotatePerSec = 1080f;
    public float stopDistance = 0.5f;

    [Tooltip("쫓아갈 대상(target)")]
    public CharacterControl target;


}

