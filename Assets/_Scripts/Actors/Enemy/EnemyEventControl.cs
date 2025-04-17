
using System.Collections;
using System.Linq;
using UnityEngine;
using CustomInspector;



public class EnemyEventControl : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    [SerializeField] EventEnemySpawnAfter eventEnemySpawnAfter;

    [SerializeField] EventSensorSightEnter eventSensorSightEnter;
    [SerializeField] EventSensorSightExit eventSensorSightExit;

    [SerializeField] EventSensorAttackEnter eventSensorAttackEnter;
    [SerializeField] EventSensorAttackExit eventSensorAttackExit;
    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion

    private CharacterControl owner;

    void Start()
    {      
        if (TryGetComponent(out owner) == false)
            Debug.LogWarning("EnemyEventControl ] CharacterControl 없음");

        owner.Visible(false);
    }

    private void OnEnable()
    {
        eventEnemySpawnAfter.Register(OneventEnemySpawnAfter); 
        eventSensorSightEnter.Register(OneventSensorSightEnter);
        eventSensorSightExit.Register(OneventSensorSightExit);
        eventSensorAttackEnter.Register(OneventSensorAttackEnter);
        eventSensorAttackExit.Register(OneventSensorAttackExit);
    }

    private void OnDisable()
    {
        eventEnemySpawnAfter.Unregister(OneventEnemySpawnAfter);  
        eventSensorSightEnter.Unregister(OneventSensorSightEnter); 
        eventSensorSightExit.Unregister(OneventSensorSightExit);
        eventSensorAttackEnter.Unregister(OneventSensorAttackEnter);
        eventSensorAttackExit.Unregister(OneventSensorAttackExit);
    }


#region EVENT-SPAWNAFTER
    void OneventEnemySpawnAfter(EventEnemySpawnAfter e)
    {
        if (owner != e.character)
            return;

        StartCoroutine(SpawnSequence(e));
    }

    IEnumerator SpawnSequence(EventEnemySpawnAfter e)
    {
        yield return new WaitUntil(()=> owner.Profile != null);

        // Enemy 컨트롤(owner)에 Actor Profile (데이터) 전달한다.                
        var model = owner.Profile.models.Random();

        // 모델 생성한 후 _MODEL_ 슬롯에 붙인다.
        if (model == null)
            Debug.LogError($"EnemyEventControl ] 모델 없음");

        var clone = Instantiate(model, owner.model);

        
        // 애니메이터 아바타 연결
        if (owner.Profile.avatar == null)
            Debug.LogError($"EnemyEventControl ] 아바타 없음");

        owner.animator.avatar = owner.Profile.avatar;

        // 1초 후 등장 파티클 발생
        yield return new WaitForSeconds(1f);

        PoolManager.I.Spawn(e.particleSpawn, transform.position, Quaternion.identity, null);
        

        // 캐릭터 등장 연출
        yield return new WaitForSeconds(0.2f);
        
        owner.Visible(true);
                

        // 1초 후 캐릭터 어빌리티 부여
        yield return new WaitForSeconds(1f);

        foreach( var dat in owner.Profile.abilities )
            owner.ability.Add(dat);


        yield return new WaitForEndOfFrame();

        // 커서 Selectable 연결
        if (TryGetComponent(out CursorSelectable sel))
            sel.SetupRenderer();


        // ui 출현
        yield return new WaitForEndOfFrame();
        owner.ui.Show(true);

        
//TEMPCODE
        yield return new WaitForEndOfFrame();
        owner.ability.Activate(AbilityFlag.Wander, true, null);
//TEMPCODE

    }

#endregion



    void OneventSensorSightEnter(EventSensorSightEnter e)
    {
        if (owner != e.from) return;
        
        owner.ability.Activate(AbilityFlag.Trace, true, e.to);
    }

    void OneventSensorSightExit(EventSensorSightExit e)
    {
        if (owner != e.from) return;

        owner.ability.Activate(AbilityFlag.Wander, true, null);
    }


    void OneventSensorAttackEnter(EventSensorAttackEnter e)
    {
        if (owner != e.from) return;

        owner.ability.Activate(AbilityFlag.Attack, true, e.to);
    }

    void OneventSensorAttackExit(EventSensorAttackExit e)
    {
        if (owner != e.from) return;

        owner.ability.Activate(AbilityFlag.Trace, true, e.to);
    }

}
