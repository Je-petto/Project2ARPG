
using UnityEngine;
using CustomInspector;
using System.Collections;


public class CharacterEventControl : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    [SerializeField] EventCameraSwitch eventCameraswitch;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion

    private CharacterControl cc;

    void Start()
    {      
        if (TryGetComponent(out cc) == false)
            Debug.LogWarning("GameEventControl ] CharacterControl 없음");

        cc.Visible(false);
    }

    private void OnEnable()
    {
        eventPlayerSpawnAfter.Register(OneventPlayerSpawnAfter);
        eventCameraswitch.Register(OneventCameraSwitch);
    }

    private void OnDisable()
    {
        eventPlayerSpawnAfter.Unregister(OneventPlayerSpawnAfter);
        eventCameraswitch.Unregister(OneventCameraSwitch);
    }


    void OneventCameraSwitch(EventCameraSwitch e)
    {
        if (e.inout)
            cc.ability.Deactivate(AbilityFlag.MoveKeyboard);
        else
            cc.ability.Activate(AbilityFlag.MoveKeyboard);
    }


    void OneventPlayerSpawnAfter(EventPlayerSpawnAfter e)
    {
        StartCoroutine(SpawnSequence(e));
    }

    IEnumerator SpawnSequence(EventPlayerSpawnAfter e)
    {
        yield return new WaitUntil(()=> e.actorProfile != null && e.actorProfile.model != null);

        // 캐릭터 컨트롤(cc)에 ActorProfile (캐릭터 데이터) 전달한다.
        cc.profile = e.actorProfile;

        // 플레이어 모델 생성한 후 _MODEL_ 슬롯에 붙인다.
        if (e.actorProfile.model == null)
            Debug.LogError($"CharacterEventControl ] 모델 없음");

        Instantiate(e.actorProfile.model, cc.model);

        
        // 플레이어 애니메이터 아바타 연결
        if (e.actorProfile.avatar == null)
            Debug.LogError($"CharacterEventControl ] 아바타 없음");

        cc.animator.avatar = e.actorProfile.avatar;


        // 1초 후 등장 파티클 발생
        yield return new WaitForSeconds(1f);

        PoolManager.I.Spawn(e.particleSpawn, transform.position, Quaternion.identity, null);
        

        // 캐릭터 등장 연출
        yield return new WaitForSeconds(0.2f);
        
        cc.Visible(true);
        cc.Animate(cc._SPAWN, 0f);


        // 1초 후 캐릭터 어빌리티 부여
        yield return new WaitForSeconds(1f);

        foreach( var dat in e.actorProfile.abilities )
            cc.ability.Add(dat, true);
    }

}
