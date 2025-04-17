using System.Collections;
using UnityEngine;
using CustomInspector;


public class SpawnerPlayer : Spawner
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;
    
    // 플레이어 스폰 관련
    [SerializeField] EventPlayerSpawnBefore eventPlayerspawnBefore;
    [SerializeField] EventPlayerSpawnAfter eventPlayerspawnAfter;
   
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion


    void OnEnable()
    {
        // 이벤트가 등록 되면 발동, 등록 안하면 작동안함 ( 트리거 역할 )
        eventPlayerspawnBefore?.Register(OneventPlayerSpawnBefore);        
    }

    void OnDisable()
    {
        eventPlayerspawnBefore?.Unregister(OneventPlayerSpawnBefore);        
    }

    CharacterControl _character;
    CursorControl _cursor;
    void OneventPlayerSpawnBefore(EventPlayerSpawnBefore e)
    {        
        // 플레이어용 카메라 만든다.
        CameraControl camera = Instantiate(e.PlayerCamera);                

        // 플레이어 캐릭터 만든다.
        Quaternion rot = Quaternion.LookRotation(spawnpoint.forward);
        _character = Instantiate(e.PlayerCharacter, spawnpoint.position, rot);

        // 플레이어 캐릭터에 프로파일 연결한다.
        _character.Profile = actorProfile; 
        // 플레이어의 상태를 프로파일과 동기화 한다.
        _character.State.Set(actorProfile);

        // 플레이어용 마우스 커서를 만든다.
        _cursor = Instantiate(e.PlayerCursor);
        _cursor.eyePoint = _character.eyepoint;

        StartCoroutine(SpawnAfter());
    }

    IEnumerator SpawnAfter()
    {
        yield return new WaitForEndOfFrame();

        // 캐릭터 틀 생성 후, After 이벤트 발동 ( 내용을 채운다 )
        eventPlayerspawnAfter.character = _character;
        eventPlayerspawnAfter.eyePoint = _character.eyepoint;
        eventPlayerspawnAfter.cursorFixedPoint = _cursor.CursorFixedPoint;
        eventPlayerspawnAfter.Raise();
    }
      
}
