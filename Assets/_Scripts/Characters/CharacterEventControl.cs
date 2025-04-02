using UnityEngine;
using CustomInspector;
using System.Collections;

public class CharacterEventControl : MonoBehaviour
{
#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;
    [SerializeField] EventCameraSwitch eventCameraSwitch;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    CharacterControl cc;
#endregion


    void Start()
    {
        if (TryGetComponent(out cc) == false)
            Debug.LogWarning("EventControl ] CharacterControl 없음");
    }

    void OnEnable()
    {

        eventPlayerSpawnAfter.Register(OneventPlayerSpawnAfter);
        eventCameraSwitch.Register(OneventCameraSwitch);
    }

    void OnDisable()
    {
        eventPlayerSpawnAfter.Unregister(OneventPlayerSpawnAfter);
        eventCameraSwitch.Unregister(OneventCameraSwitch);
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
        yield return new WaitForSeconds(1f);
        PoolManager.I.Spawn(e.particleSpawn, transform.position, Quaternion.identity, null);
        yield return new WaitForSeconds(0.2f);
        cc.Visable(true);
        cc.Animate(cc._SPAWN, 0f);
    }

    // Unity(비동기 지원 안함 -> 싱글쓰레드)

    //비동기 코드(Async)
    // 1. 코루틴 ( Co-routine )
    // 2. Invoke
    // 3. asynce / await
    // 4. Awaitable
    // 5. CySharp
    // 6. DoTween - DoVirtual.Delay ( 3f, ()=> {})


}
