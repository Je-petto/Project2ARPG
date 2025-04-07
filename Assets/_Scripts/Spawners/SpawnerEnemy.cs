using System.Collections;
using UnityEngine;
using CustomInspector;

public class SpawnerEnemy : Spawner
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    // 적 캐릭터 스폰 관련
    [SerializeField] EventEnemySpawnBefore eventEnemySpawnBefore;
    [SerializeField] EventEnemySpawnAfter eventEnemySpawnAfter;    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion
    
    void OnEnable()
    {
        // 이벤트가 등록되면 발동, 등록 안하면 작동안함 (트리거 역할)
        eventEnemySpawnBefore?.Register(OneventEnemySpawnBefore);
    }

    void OnDisable()
    {
        eventEnemySpawnBefore?.Unregister(OneventEnemySpawnBefore);
    }
    EnemyControl _enemyControl;
    void OneventEnemySpawnBefore(EventEnemySpawnBefore e)
    {
        _enemyControl = Instantiate(e.enemyCharacter);
        Quaternion rot = Quaternion.LookRotation(spawnpoint.forward);
        _enemyControl.transform.SetPositionAndRotation(spawnpoint.position, rot);

        StartCoroutine(SpawnAfter());
    }

    IEnumerator SpawnAfter()
    {
        yield return new WaitForEndOfFrame();

        eventEnemySpawnAfter.eyePoint = _enemyControl.eyepoint;
        eventEnemySpawnAfter.actorProfile = actorProfile;
        eventEnemySpawnAfter?.Raise();  
    }    
}    

