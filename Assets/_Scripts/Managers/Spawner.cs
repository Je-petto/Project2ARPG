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

    [Space(10)]
    [SerializeField] ActorProfile actorProfile;

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
        // 캐릭터 틀 만든다
        CameraControl camera = Instantiate(e.PlayerCamera);

        CharacterControl character = Instantiate(e.PlayerCharactor);
        Quaternion rot = Quaternion.LookRotation(spawnpoint.forward);
        character.transform.SetPositionAndRotation(spawnpoint.position, rot);        
        
        CursorControl cursor = Instantiate(e.PlayerCursor);
        cursor.eyePoint = character.eyepoint;

        // 캐릭터 틀 생성 후, After 이벤트 발동 (내용을 채운다)
        eventPlayerSpawnAfter.eyePoint = character.eyepoint;
        eventPlayerSpawnAfter.cursorFixedPoint = cursor.CursorFixedPoint;
        eventPlayerSpawnAfter.actorProfile = actorProfile;
        eventPlayerSpawnAfter?.Raise();

    }
}
