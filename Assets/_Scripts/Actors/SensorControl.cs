using CustomInspector;
using UnityEngine;

public class SensorControl : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    [SerializeField] EventSensorTargetEnter eventSensorTargetEnter;
    [SerializeField] EventSensorTargetExit eventSensorTargetExit;
    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion

    [Space(10)]
    [Tooltip ("시아 범위")]
    [SerializeField] float sightRange = 5f;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] string targetTag;

    [Space(15)]
    // 센서 타겟
    [ReadOnly] public CharacterControl target;

    // 자신의 CharacterControl
    private CharacterControl owner;

    void Start()
    {
        owner = GetComponentInParent<CharacterControl>();
        if(owner == null)
            Debug.LogWarning("SensorControl] CharacterControl 없음");
    }


    CharacterControl _prev;
    void Update()
    {        
        var cols = Physics.OverlapSphere(transform.position, sightRange, targetLayer);
        foreach( var c in cols)
        {
            if (c.tag == targetTag)
            {
                target = c.GetComponentInParent<CharacterControl>();
                TargetEnter();               
                return;
            }
        }

        TargetExit();
    }

    public void TargetEnter()
    {
        if (_prev == target || target == null)
            return;

        _prev = target;

        // Debug.Log($"Target Enter: {target.Profile.alias}");
        eventSensorTargetEnter.from = owner;
        eventSensorTargetEnter.to = target;
        eventSensorTargetEnter?.Raise();
    }

    public void TargetExit()
    {
        if (_prev == null || target == null) return;

        _prev = null;

        eventSensorTargetExit.from = owner;
        eventSensorTargetExit.to = target;
        eventSensorTargetExit?.Raise();
    }
}
