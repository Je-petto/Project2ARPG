using UnityEngine;

public class Spawner : MonoBehaviour
{
#region  EVENTS
    [Space(15)]
    [SerializeField] EventPlayerSpawnBefore eventPlayerSpawnBefore;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
#endregion
    
    public float radius = 2f;
    public float linelength = 2f;

    void OnEnable()
    {
        eventPlayerSpawnBefore.Register(OneventPlayerSpawnBefore);        
    }

    void OnDisable()
    {
        eventPlayerSpawnBefore.Register(OneventPlayerSpawnBefore);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, radius);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * linelength);
    }

    void OneventPlayerSpawnBefore(EventPlayerSpawnBefore e)
    {        
        CameraControl camera = Instantiate(e.PlayerCamera);

        CharacterControl character = Instantiate(e.PlayerCharactor);
        Quaternion rot = Quaternion.LookRotation(transform.forward);
        character.transform.SetPositionAndRotation(transform.position, rot);        
        
        CursorControl cursor = Instantiate(e.PlayerCursor);
        cursor.EyePoint = character.eyepoint;

        // 캐릭터 생성 후, After 이벤트 발동
        eventPlayerSpawnAfter.eyepoint = character.eyepoint;
        eventPlayerSpawnAfter.cursorpoint = cursor.CursorPoint;
        eventPlayerSpawnAfter?.Raise();
    }
}
