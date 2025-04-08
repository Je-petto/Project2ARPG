using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "Abilities/Wander")]

public class AbilityWanderData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Wander;
    public override Ability CreateAbility(CharacterControl owner) =>  new AbilityWander(this, owner);

    [ReadOnly] public float movePerSec = 5f;
    [ReadOnly] public float rotatePerSec = 1080f;
    public float stopDistance = 0.5f;

    [Tooltip("배회할 범워(radius)")]
    public float wanderRadius = 5f;

    [Tooltip("도착 후 대기 시간(seconds)")]
    public float wanderStay = 2f;
}

