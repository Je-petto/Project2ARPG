using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "Abilities/Move Mouse")]

public class AbilityMoveMouseData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.MoveMouse;
    public override Ability CreateAbility(CharacterControl owner) => new AbilityMoveMouse(this, owner);
    public float movePerSec = 5f;
    public float rotatePerSec = 1080f;
    public float stopdistance = 0.5f;
    
    [Tooltip("min: RunToStop 모션 발동 지점, max: RunToStop 발동할 거리")]
    [AsRange(0f,10f)] public Vector2 runtostopDistance;

    [Space(20)]
    public ParticleSystem marker; //3d picking(피킹) 마커 오브젝트


}

