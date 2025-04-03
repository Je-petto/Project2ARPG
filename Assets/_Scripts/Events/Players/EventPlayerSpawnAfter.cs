using UnityEngine;
using CustomInspector;


[CreateAssetMenu(menuName = "GameEvent/EventPlayerSpawnAfter")]
public class EventPlayerSpawnAfter : GameEvent<EventPlayerSpawnAfter>
{
    public override EventPlayerSpawnAfter Item => this;

    [ReadOnly] public Transform eyePoint;
    [ReadOnly] public Transform cursorFixedPoint;    
    [ReadOnly] public ActorProfile actorProfile;
    

    [Tooltip("플레이어 스폰시 발동 파티클")]
    public PoolableParticle particleSpawn;
}
