using UnityEngine;
using CustomInspector;


[CreateAssetMenu(menuName = "GameEvent/EventPlayerSpawnAfter")]
public class EventPlayerSpawnAfter : GameEvent<EventPlayerSpawnAfter>
{
    public override EventPlayerSpawnAfter Item => this;

    [ReadOnly] public CharacterControl character;
    [ReadOnly] public Transform eyePoint;
    [ReadOnly] public Transform cursorFixedPoint;
    

    [Tooltip("플레이어 스폰시 발동 파티클")]
    public PoolableParticle particleSpawn;
}
