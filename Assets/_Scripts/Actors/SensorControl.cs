using UnityEngine;
using CustomInspector;

public class SensorControl : MonoBehaviour
{
    #region 이벤트
    [HorizontalLine("이벤트"), HideField] public bool _h0;

    [SerializeField] EventSensorSightEnter eventSensorSightEnter;
    [SerializeField] EventSensorSightExit eventSensorSightExit;
    [SerializeField] EventSensorAttackEnter eventSensorAttackEnter;
    [SerializeField] EventSensorAttackExit eventSensorAttackExit;

    [Space(10), HorizontalLine(color:FixedColor.Cyan), HideField] public bool _h1;
#endregion

    [Tooltip ("시야 범위")]
    [SerializeField] float sightRange = 10f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] LayerMask targetlayer;
    [SerializeField] string targetTag;

    [Space(10)]
    // 타켓 CharacterControl
    [ReadOnly] public CharacterControl target;

    //자신의 CharacterControl
    private CharacterControl owner;

    void Start()
    {
        owner = GetComponentInParent<CharacterControl>();
        if (owner == null)
            Debug.LogWarning("커서 컨트롤 ] 캐릭터 컨트롤 없음. ");

        InvokeRepeating("checkOverlap", 0f, 0.1f);
    }

    void checkOverlap()
    {
        // 시아 거리리 안에 들어왔는지 판단
        var cols = Physics.OverlapSphere(owner.eyepoint.position, sightRange, targetlayer);
        foreach(var c in cols)
        {
            if (c.tag == targetTag)
            {
                target = c.GetComponentInParent<CharacterControl>();
                SightEnter();

                    var d = Vector3.Distance(target.eyepoint.position, owner.eyepoint.position);
                    if ( d <= attackRange)
                        AttackEnter();
                    else
                        AttackExit();
                    return;
            }
        }
        AttackExit();
        SightExit();
    }


    CharacterControl _predSight;
    public void SightEnter()
    {
        if (_predSight == target || target == null) 
            return;
        
        _predSight = target;

        Debug.Log($" Target Enter : {target.Profile.alias}");
        
        eventSensorSightEnter.from = owner;
        eventSensorSightEnter.to = target;
        eventSensorSightEnter.Raise();    
    }

    public void SightExit()
    {        
        if (_predSight == null || target == null) 
            return;

        _predSight = null;

        Debug.Log($" Target Exit : {target.Profile.alias}");

        eventSensorSightExit.from = owner;
        eventSensorSightExit.to = target;
        eventSensorSightExit.Raise();
    }


    CharacterControl _prevAttack;
    // 공격 범위 안에 있을때.
    private void AttackEnter()
    {
        if (_prevAttack == target || target == null) 
            return;

        _prevAttack = target;
    
        eventSensorAttackEnter.from = owner;
        eventSensorAttackEnter.to = target;
        eventSensorAttackEnter.Raise();
    }

    // 공격 범위 밖에 있을때.
    private void AttackExit()
    {
        if (_prevAttack == null || target == null) 
            return;

        _prevAttack = null;

        eventSensorAttackExit.from = owner;
        eventSensorAttackExit.to = target;
        eventSensorAttackExit.Raise();
    }

    // 장애물 체크
    //if (Physics.Raycast(owner.eyepoint.position, owner.eyepoint.forward ,out var hit, sightRange))

    //1. "Ignore Raycast" 레이어 만 필터. 
    //if(Physics.OverlapSphere(owner.eyepoint.position, sightRange, LayerMask.NameToLayer("Ignore Raycast")); //필요할때 체크해서 사용. LayerMask = 이스펙터의 Layer

    //     Debug.Log($" 오버렙스 : {overlaps.Length}");

    //     //2. Tag : "Player" 만 필터
    //     foreach (var c in overlaps)
    //     {
    //         if (c.tag == "player")
    //         {
    //             target = c.GetComponentInParent<CharacterControl>();
    //             Debug.Log($"타켓 설정 : {target}");
    //         }
    //     }


}