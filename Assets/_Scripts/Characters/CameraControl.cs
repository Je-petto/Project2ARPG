using Unity.Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
#region  EVENTS
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
#endregion

    [SerializeField] CinemachineTargetGroup targetGroup;
    
    void OnEnable()
    {
        eventPlayerSpawnAfter.Register(OneventPlayerSpawnAfter);

    }

    void OnDisable()
    {
        eventPlayerSpawnAfter.Unregister(OneventPlayerSpawnAfter);
    }

    void OneventPlayerSpawnAfter(EventPlayerSpawnAfter e)
    {
        targetGroup.Targets.Clear();
        
        CinemachineTargetGroup.Target main = new CinemachineTargetGroup.Target();
        main.Object = e.eyepoint;
        main.Weight = 0.9f;
        targetGroup.Targets.Add(main);

        CinemachineTargetGroup.Target sub = new CinemachineTargetGroup.Target();
        sub.Object = e.cursorpoint;
        sub.Weight = 0.1f;
        targetGroup.Targets.Add(sub);


    }
}
