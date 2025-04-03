using Unity.Cinemachine;
using UnityEngine;
using CustomInspector;

public class CameraControl : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;

    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
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
        main.Object = e.eyePoint;
        main.Weight = 0.9f;
        targetGroup.Targets.Add(main);

        CinemachineTargetGroup.Target sub = new CinemachineTargetGroup.Target();
        sub.Object = e.cursorFixedPoint;
        sub.Weight = 0.1f;
        targetGroup.Targets.Add(sub);
    }
}
