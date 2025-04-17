
using System.Collections;
using System.Linq;
using UnityEngine;
using CustomInspector;
using Unity.VisualScripting;



public class CharacterEventControl : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    [SerializeField] EventCameraSwitch eventCameraswitch;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    [SerializeField] EventDeath eventDeath;
    [SerializeField] EventAttackAfter eventAttackAfter;

    [SerializeField] EventSensorSightEnter eventSensorSightEnter;
    [SerializeField] EventSensorSightExit eventSensorSightExit;

    [SerializeField] EventCursorHover eventCursorHover;
    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion

    private CharacterControl owner;

    void Start()
    {      
        if (TryGetComponent(out owner) == false)
            Debug.LogWarning("GameEventControl ] CharacterControl 없음");

        owner.Visible(false);
    }

    private void OnEnable()
    {
        eventPlayerSpawnAfter.Register(OneventPlayerSpawnAfter);
        eventDeath.Register(OneventDeath);
        eventCameraswitch.Register(OneventCameraSwitch);
        eventAttackAfter.Register(OneventAttackAfter);

        eventSensorSightEnter.Register(OneventSensorSightEnter);
        eventSensorSightExit.Register(OneventSensorSightExit);

        eventCursorHover.Register(OneventCursorHover);
    }

    private void OnDisable()
    {
        eventPlayerSpawnAfter.Unregister(OneventPlayerSpawnAfter);
        eventDeath.Unregister(OneventDeath);
        eventCameraswitch.Unregister(OneventCameraSwitch);
        eventAttackAfter.Unregister(OneventAttackAfter);

        eventSensorSightEnter.Unregister(OneventSensorSightEnter);
        eventSensorSightExit.Unregister(OneventSensorSightExit);

        eventCursorHover.Unregister(OneventCursorHover);
    }


    void OneventCameraSwitch(EventCameraSwitch e)
    {
        if (e.inout)
            owner.ability.Deactivate(AbilityFlag.MoveKeyboard);
        else
            owner.ability.Activate(AbilityFlag.MoveKeyboard, false, null);
    }


#region SPAWNS

    void OneventPlayerSpawnAfter(EventPlayerSpawnAfter e)
    {
        StartCoroutine(SpawnSequence(e));
    }

    IEnumerator SpawnSequence(EventPlayerSpawnAfter e)
    {
        yield return new WaitUntil(()=> owner.Profile != null);

        // 캐릭터 컨트롤(CC)에 Actor Profile (캐릭터 데이터) 전달한다.
        var model = owner.Profile.models.Random();

        // 플레이어 모델 생성한 후 _MODEL_ 슬롯에 붙인다.
        if (model == null)
            Debug.LogError($"CharacterEventControl ] 모델 없음");

        var clone = Instantiate(model, owner.model);


        // 생성한 모델 ( Wrapper ) 의 Feedback 연결
        var feedback = clone.GetComponentInChildren<FeedbackControl>();
        if (feedback != null)
            owner.feedback = feedback;


        // Skinned 메시만 실루엣 처리 한다.
        clone.GetComponentsInChildren<SkinnedMeshRenderer>().ToList().ForEach( m => 
        {
            m.gameObject.layer = LayerMask.NameToLayer("Silhouette");
        });

        
        // 플레이어 애니메이터 아바타 연결
        if (owner.Profile.avatar == null)
            Debug.LogError($"CharacterEventControl ] 아바타 없음");

        owner.animator.avatar = owner.Profile.avatar;



        // 1초 후 등장 파티클 발생
        yield return new WaitForSeconds(1f);

        PoolManager.I.Spawn(e.particleSpawn, transform.position, Quaternion.identity, null);
        

        // 캐릭터 등장 연출
        yield return new WaitForSeconds(0.2f);
        
        owner.Visible(true);
        owner.Animate("SPAWN", 0f);


        // 1초 후 캐릭터 어빌리티 부여
        yield return new WaitForSeconds(1f);

        foreach( var dat in owner.Profile.abilities )
            owner.ability.Add(dat, true);        
    }

    void OneventDeath(EventDeath e)
    {
        // 죽은 캐릭터가 본인이 아니면 패스
        if (owner != e.target)
            return;
        
        owner.ik.isTarget = false;

        owner.Animate("DEATH", 0.2f);
        owner.ability.RemoveAll();
    }

#endregion


#region DAMAGES


    void OneventSensorSightEnter(EventSensorSightEnter e)
    {
        if (owner != e.from) return;
        
    }

    void OneventSensorSightExit(EventSensorSightExit e)
    {
        // 바라보는 자신과 from 이 다르거나 , 바라볼 타겟과 to 가 다르면 , RETURN
        if (owner != e.from) return;

        owner.ik.target = null;
        owner.ik.isTarget = false;
    }

    void OneventAttackAfter(EventAttackAfter e)
    {
        // 플레이어가 맞는지 체크
        if (owner != e.to)
            return;

        // 플레이어 의 Damage Ability 발동
        // object : EventAttackAfter 
        owner.ability.Activate(AbilityFlag.Damage, false, e);
    }


    // 플레이어가 커서 타겟된 위치를 바라본다
    void OneventCursorHover(EventCursorHover e)
    {
        owner.ik.isTarget = true;
        owner.ik.target = e.target.eyepoint;

        owner.LookatY(e.target.eyepoint.position);
    }


#endregion

}
