using UnityEngine;


[CreateAssetMenu(menuName = "GameEvent/EventPlayerSpawnBefore")]
public class EventPlayerSpawnBefore : GameEvent<EventPlayerSpawnBefore>
{
    public override EventPlayerSpawnBefore Item => this;

    public CharacterControl PlayerCharactor;
    public CameraControl PlayerCamera;    
    public CursorControl PlayerCursor;


    


}
