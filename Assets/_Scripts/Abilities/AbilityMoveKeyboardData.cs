using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Move Keyboard")]

public class AbilityMoveKeyboardData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.MoveKeyboard;
    public override Ability CreateAbility(CharacterControl owner) => new AbilityMoveKeyboard(this, owner);
    public float movePerSec = 5f;
    public float rotatePerSec = 1080f;


}

