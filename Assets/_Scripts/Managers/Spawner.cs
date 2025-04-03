using UnityEngine;
using CustomInspector;
using System.Collections;

public class Spawner : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;
    
    [SerializeField] EventPlayerSpawnBefore eventPlayerspawnBefore;
    [SerializeField] EventPlayerSpawnAfter eventPlayerspawnAfter;
    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion

    
    [Space(20)]
    public Transform spawnpoint;    


    [Space(20)]
    [SerializeField] ActorProfile actorProfile;


    void OnEnable()
    {
        eventPlayerspawnBefore.Register(OneventPlayerSpawnBefore);
    }

    void OnDisable()
    {
        eventPlayerspawnBefore.Unregister(OneventPlayerSpawnBefore);
    }





    CharacterControl _character;
    CursorControl _cursor;
    void OneventPlayerSpawnBefore(EventPlayerSpawnBefore e)
    {        
        // 캐릭터 틀 만든다.
        CameraControl camera = Instantiate(e.PlayerCamera);                

        _character = Instantiate(e.PlayerCharacter);
        Quaternion rot = Quaternion.LookRotation(spawnpoint.forward);
        _character.transform.SetPositionAndRotation(spawnpoint.position, rot);
        
        _cursor = Instantiate(e.PlayerCursor);
        _cursor.eyePoint = _character.eyepoint;

        StartCoroutine(delayevent());
    }

    IEnumerator delayevent()
    {
        yield return new WaitForEndOfFrame();

        // 캐릭터 틀 생성 후, After 이벤트 발동 ( 내용을 채운다 )
        eventPlayerspawnAfter.eyePoint = _character.eyepoint;
        eventPlayerspawnAfter.cursorFixedPoint = _cursor.CursorFixedPoint;
        eventPlayerspawnAfter.actorProfile = actorProfile;
        eventPlayerspawnAfter?.Raise();   
    }
}
