using UnityEngine;

public class GameEventControl : MonoBehaviour
{
    [SerializeField] GameEventCameraSwitch eventCameraSwitch;
    CharacterControl cc;

    void Start()
    {
        if (TryGetComponent(out cc) == false)
            Debug.LogWarning("GameEventControl ] CharacterControl 없음");
    }

    void OnEnable()
    {
        eventCameraSwitch.Register(OneventCameraSwitch);
    }

    void OnDisable()
    {
        eventCameraSwitch.Unregister(OneventCameraSwitch);
    }

    void OneventCameraSwitch(GameEventCameraSwitch e)
    {
        if (e.inout)
            cc.ability.Deactivate(AbilityFlag.MoveKeyboard);
        else   
            cc.ability.Activate(AbilityFlag.MoveKeyboard);
    }
}
