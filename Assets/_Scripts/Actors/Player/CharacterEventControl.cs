using System.Collections;
using System.Linq;
using UnityEngine;
using CustomInspector;

public class CharacterEventControl : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    [SerializeField] EventCameraSwitch eventCameraswitch;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    
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
            owner.ability.Deactivate(AbilityFlag.MoveKeyboard);
        else
            owner.ability.Activate(AbilityFlag.MoveKeyboard, false, null);
    }


    void OneventPlayerSpawnAfter(EventPlayerSpawnAfter e)
    {
        StartCoroutine(SpawnSequence(e));
    }

    IEnumerator SpawnSequence(EventPlayerSpawnAfter e)
    {
        yield return new WaitUntil(()=> owner.Profile != null);

        // 캐릭터 컨트롤에 Actor 프로파일을 전환
        var model = owner.Profile.models.Random();

        // 플레이어 모델 생성한 후 _MODEL_ 슬롯에 붙인다.
        if (model == null)
            Debug.LogError($"CharacterEventControl ] 모델 없음");

        var clone = Instantiate(model, owner.model);

        //Skinned Mesh 만 실루엣 처리
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
        owner.Animate(AnimatorHashes._SPAWN, 0f);


        // 1초 후 캐릭터 어빌리티 부여
        yield return new WaitForSeconds(1f);

        foreach( var dat in owner.Profile.abilities )
            owner.ability.Add(dat, true);
    }

}
