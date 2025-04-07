using System.Collections;
using UnityEngine;
using CustomInspector;

public class Scenario : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    public EventPlayerSpawnBefore eventPlayerSpawnBefore;
    public EventEnemySpawnBefore eventEnemySpawn;

    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion


    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        // 플레이어 스폰 전 동작
        eventPlayerSpawnBefore?.Raise();

        yield return new WaitForEndOfFrame();
        // 적 캐릭터 스폰
        eventEnemySpawn?.Raise();


        // 아이템 스폰
        
    }
    
}

