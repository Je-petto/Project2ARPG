using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventSensorSightEnter")]
public class EventSensorSightEnter : GameEvent<EventSensorSightEnter>
{
    //F2 누루면 다른 연결된 script에서도 이름 바뀜
    //Ctrl + H 누루면 goekd script에서만 이름 바뀜
    public override EventSensorSightEnter Item => this;

    // From (발견자)
    public CharacterControl from;

    // To (피발견자)
    public CharacterControl to;
}
