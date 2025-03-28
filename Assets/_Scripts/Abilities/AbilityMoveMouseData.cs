using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Move Mouse")]

public class AbilityMoveMouseData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.MoveMouse;
    public override Ability CreateAbility(CharacterControl owner) => new AbilityMoveMouse(this, owner);
    public float movePerSec = 5f;
    public float rotatePerSec = 1080f;
    public float stopdistance = 0.5f;
    public float runtostopDistance = 1f;

    [Space(20)]
    public ParticleSystem marker; //3d picking(피킹) 마커 오브젝트


}

