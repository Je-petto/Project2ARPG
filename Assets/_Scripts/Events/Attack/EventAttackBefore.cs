using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventAttackBefore")]
public class EventAttackBefore : GameEvent<EventAttackBefore>
{
    //F2 누루면 다른 연결된 script에서도 이름 바뀜
    //Ctrl + H 누루면 goekd script에서만 이름 바뀜
    public override EventAttackBefore Item => this;

    // From (가해자)
    [ReadOnly] public CharacterControl from;
}
