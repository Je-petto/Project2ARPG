using UnityEngine;
using CustomInspector;

public class Spawner : MonoBehaviour
{
#region EVENTS
    [Space(10)]
    [HorizontalLine("EVENTS"),HideField] public bool _h0;
    [Foldout, SerializeField] EventPlayerSpawnBefore eventPlayerSpawnBefore;
    [Foldout, SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
#endregion
    
    [Space(15),HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
    public Transform spawnpoint;

    void OnEnable()
    {
        eventPlayerSpawnBefore.Register(OneventPlayerSpawnBefore);        
    }

    void OnDisable()
    {
        eventPlayerSpawnBefore.Register(OneventPlayerSpawnBefore);
    }


    void OneventPlayerSpawnBefore(EventPlayerSpawnBefore e)
    {        
        CameraControl camera = Instantiate(e.PlayerCamera);

        CharacterControl character = Instantiate(e.PlayerCharactor);
        Quaternion rot = Quaternion.LookRotation(spawnpoint.forward);
        character.transform.SetPositionAndRotation(spawnpoint.position, rot);        
        
        CursorControl cursor = Instantiate(e.PlayerCursor);
        cursor.EyePoint = character.eyepoint;

        // 캐릭터 생성 후, After 이벤트 발동
        eventPlayerSpawnAfter.eyepoint = character.eyepoint;
        eventPlayerSpawnAfter.cursorpoint = cursor.CursorPoint;
        eventPlayerSpawnAfter?.Raise();
    }
}
