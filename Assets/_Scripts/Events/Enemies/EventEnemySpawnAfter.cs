using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "GameEvent/EventEnemySpawnAfter")]
public class EventEnemySpawnAfter : GameEvent<EventEnemySpawnAfter>
{
    //F2 누루면 다른 연결된 script에서도 이름 바뀜
    //Ctrl + H 누루면 goekd script에서만 이름 바뀜
    public override EventEnemySpawnAfter Item => this;

    [ReadOnly] public CharacterControl character;
    [ReadOnly] public Transform eyePoint;

    [Tooltip("플레이어 스폰시 발동 파티클")]
    public PoolableParticle particleSpawn;

}
