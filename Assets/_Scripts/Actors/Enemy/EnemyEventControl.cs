using System.Collections;
using System.Linq;
using UnityEngine;
using CustomInspector;

public class EnemyEventControl : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    [SerializeField] EventEnemySpawnAfter eventEnemySpawnAfter;
    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion

    private CharacterControl cc;

    void Start()
    {      
        if (TryGetComponent(out cc) == false)
            Debug.LogWarning("EnemyEventControl ] CharactorControl 없음");

        cc.Visible(false);
    }

    private void OnEnable()
    {
        eventEnemySpawnAfter.Register(OneventEnemySpawnAfter);

    }

    private void OnDisable()
    {
        eventEnemySpawnAfter.Unregister(OneventEnemySpawnAfter);
    }


    void OneventEnemySpawnAfter(EventEnemySpawnAfter e)
    {
        StartCoroutine(SpawnSequence(e));
    }

    IEnumerator SpawnSequence(EventEnemySpawnAfter e)
    {
        
        yield return new WaitUntil(()=> e.actorProfile != null && e.actorProfile.model != null);

        // Enemy 컨트롤(cc)에 Actor Profile (Enemy 데이터) 전달한다.
        cc.Profile = e.actorProfile;

        // // 플레이어 모델 생성한 후 _MODEL_ 슬롯에 붙인다.
        if (e.actorProfile.model == null)
            Debug.LogError($"EnemyEventControl ] 모델 없음");

        var clone = Instantiate(e.actorProfile.model, cc.model);

        
        // 플레이어 애니메이터 아바타 연결
        if (e.actorProfile.avatar == null)
            Debug.LogError($"EnemyEventControl ] 아바타 없음");

        cc.animator.avatar = e.actorProfile.avatar;


        // 1초 후 등장 파티클 발생
        yield return new WaitForSeconds(1f);

        PoolManager.I.Spawn(e.particleSpawn, transform.position, Quaternion.identity, null);
        

        // 캐릭터 등장 연출
        yield return new WaitForSeconds(0.2f);
        
        cc.Visible(true);


        // //1초 후 캐릭터 어빌리티 부여
        yield return new WaitForSeconds(1f);

        foreach( var dat in e.actorProfile.abilities )
            cc.ability.Add(dat, true);
    }

}
