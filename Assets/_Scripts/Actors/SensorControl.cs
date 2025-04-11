using CustomInspector;
using UnityEngine;

public class SensorControl : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    [SerializeField] EventSensorSightEnter eventSensorSightEnter;
    [SerializeField] EventSensorSightExit eventSensorSightExit;

    [SerializeField] EventSensorAttackEnter eventSensorAttackEnter;
    [SerializeField] EventSensorAttackExit eventSensorAttackExit;
    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion

    [Space(10)]
    [Tooltip ("시아 범위")]
    [SerializeField] float sightRange = 5f;
    [SerializeField] float attacRange = 1f;

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


    void Update()
    {        
        // 시아거리 안에 들어왔나?
        var cols = Physics.OverlapSphere(owner.eyepoint.position, sightRange, targetLayer);
        foreach( var c in cols)
        {
            if (c.tag == targetTag)
            {
                target = c.GetComponentInParent<CharacterControl>();
                SightEnter();               
                
                var d = Vector3.Distance(target.eyepoint.position, owner.eyepoint.position);
                if(d <= attacRange)
                    AttackEnter();
                else
                    AttackExit();

                return;
            }
        }

        AttackExit();
        SightExit();
    }


    CharacterControl _prevSight;
    public void SightEnter()
    {
        if (_prevSight == target || target == null)
            return;

        _prevSight = target;

        // Debug.Log($"Target Enter: {target.Profile.alias}");
        eventSensorSightEnter.from = owner;
        eventSensorSightEnter.to = target;
        eventSensorSightEnter?.Raise();
    }

    public void SightExit()
    {
        if (_prevSight == null || target == null) return;

        _prevSight = null;

        eventSensorSightExit.from = owner;
        eventSensorSightExit.to = target;
        eventSensorSightExit?.Raise();
    }

    CharacterControl _prevAttack;
    // 공격 범위 안에 들어왔을 때
    private void AttackEnter()
    {
        if (_prevAttack == target || target == null)
            return;

        _prevAttack = target;

        eventSensorAttackEnter.from = owner;
        eventSensorAttackEnter.to = target;
        eventSensorAttackEnter?.Raise();
    }

    // 공격 범위 벗어났을 때
    private void AttackExit()
    {
        if (_prevAttack == null || target == null)
            return;

        _prevAttack = null;

        eventSensorAttackExit.from = owner;
        eventSensorAttackExit.to = target;
        eventSensorAttackExit?.Raise();
    }

}
