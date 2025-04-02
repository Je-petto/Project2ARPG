using UnityEngine;
using CustomInspector;


[CreateAssetMenu(menuName = "GameEvent/EventPlayerSpawnAfter")]
public class EventPlayerSpawnAfter : GameEvent<EventPlayerSpawnAfter>
{
    public override EventPlayerSpawnAfter Item => this;

    [ReadOnly] public Transform eyepoint;
    [ReadOnly] public Transform cursorpoint;    


}
