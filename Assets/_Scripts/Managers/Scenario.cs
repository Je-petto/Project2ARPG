using System.Collections;
using UnityEngine;
using CustomInspector;
using Unity.VisualScripting;

public class Scenario : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    //SPAWN 관련
    public EventPlayerSpawnBefore eventPlayerSpawnBefore;
    public EventEnemySpawnBefore eventEnemySpawn;

    //DESPAWN 관련
    public EventDeath eventDeath;

    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion


    void OnEnable()
    {
        eventDeath.Register(OneventDeath);
    }

    void ODisable()
    {
        eventDeath.Unregister(OneventDeath);        
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        // 플레이어 스폰 전 동작
        eventPlayerSpawnBefore?.Raise();

        
        yield return new WaitForEndOfFrame();    
        // 적 캐릭터 스폰
        eventEnemySpawn?.Raise();

        // 아이템 스폰

        // Information UI 연출
        yield return new WaitForSeconds(1f);
        GameManager.I.ShowInfo("KILL ALL ENEMIES", 5f);        
    }
    
    void OneventDeath(EventDeath e)
    {
        if (e.target.Profile.actorType == ActorType.PLAYER)
            GameManager.I.ShowInfo("YOU DIED", 10f);
    }

}
