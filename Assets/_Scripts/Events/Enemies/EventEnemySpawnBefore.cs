using UnityEngine;


[CreateAssetMenu(menuName = "GameEvent/EventEnemySpawnBefore")]
public class EventEnemySpawnBefore : GameEvent<EventEnemySpawnBefore>
{
    //F2 누루면 다른 연결된 script에서도 이름 바뀜
    //Ctrl + H 누루면 goekd script에서만 이름 바뀜
    public override EventEnemySpawnBefore Item => this;

    [Space(20)]
    public EnemyControl enemyCharacter;

}
