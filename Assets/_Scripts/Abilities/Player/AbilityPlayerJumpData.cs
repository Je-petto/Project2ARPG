using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "Abilities/PlayerJump")]
public class AbilityPlayerJumpData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Jump;
    public override Ability CreateAbility(CharacterControl owner) => new AbilityPlayerJump(this, owner);

    [ReadOnly] public float jumpForce = 30f;   // 점프 파워
    [ReadOnly] public float jumpDuration = 0.3f; // 점프 지속 시간
    public AnimationCurve jumpCurve; // 점프 궤적
}
