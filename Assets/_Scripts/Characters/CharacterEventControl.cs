using UnityEngine;

public class CharacterEventControl : MonoBehaviour
{
    [SerializeField] GameEventCameraSwitch eventCameraSwitch;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    CharacterControl cc;


    void Start()
    {
        if (TryGetComponent(out cc) == false)
            Debug.LogWarning("GameEventControl ] CharacterControl 없음");
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

    private void OnDestroy()
    {

    }

    void OneventCameraSwitch(GameEventCameraSwitch e)
    {
        if (e.inout)
            cc.ability.Deactivate(AbilityFlag.MoveKeyboard);
        else   
            cc.ability.Activate(AbilityFlag.MoveKeyboard);
    }

    void OneventPlayerSpawnAfter(EventPlayerSpawnAfter e)
    {
        //1초 후에 보이게 처리 // 비동기(synchronous - async) 처리
        GameManager.I.DelayCallAsync(1000, ()=> cc.Visable(true)).Forget();
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
