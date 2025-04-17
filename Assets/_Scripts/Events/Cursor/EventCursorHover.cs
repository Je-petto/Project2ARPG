using UnityEngine;
using CustomInspector;


[CreateAssetMenu(menuName = "GameEvent/EventCursorHover")]
public class EventCursorHover : GameEvent<EventCursorHover>
{
    public override EventCursorHover Item => this;


    // 커서에 타겟된 Character Control
    [ReadOnly] public CharacterControl target;

}
