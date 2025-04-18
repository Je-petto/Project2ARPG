using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using UnityEngine.Events;

public struct TargetState
{
    public bool isVisible;
    public bool isArrived;
}

public class Sensor : MonoBehaviour
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

    [Header("Detection Settings")]
    public float interval = 0.5f; // Interval for detection checks
    public float detectionRadius = 5f;
    public float arrivedRadius = 3f; 
    public float fieldOfViewAngle = 60f; // New FOV angle parameter
    public LayerMask targetLayer;
    public LayerMask blockLayer;

    public string targetTag = "Enemy";
    public bool showGizmos = true;


    
    private Dictionary<CharacterControl, TargetState> visibilityStates = new Dictionary<CharacterControl, TargetState>();

    [HorizontalLine("DEBUG"),HideField] public bool _h2;
    [ReadOnly, SerializeField] private CharacterControl owner, target;

    void OnEnable()
    {
        eventEnemySpawnAfter?.Register(OneventEnemySpawnAfter);
    }
    void OnDisable()
    {
        eventEnemySpawnAfter?.Unregister(OneventEnemySpawnAfter);    
    }
    
    void OneventEnemySpawnAfter(EventEnemySpawnAfter e)
    {
        if (owner != e.character) return;

        detectionRadius  = owner.Profile.sightrange;
        arrivedRadius = owner.Profile.attackrange;
    }

    void Start()
    {
        owner = GetComponentInParent<CharacterControl>();
        if (owner == null)
            Debug.LogWarning("Sensor ] owner - CharacterControl 없음");

        InvokeRepeating("DetectTargets", 0f, interval);
    }

    void DetectTargets()
    {
        HashSet<CharacterControl> currentFrameTargets = new HashSet<CharacterControl>();
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, targetLayer);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag(targetTag))
                continue;

            target = hit.GetComponentInParent<CharacterControl>();
            if (target == null)
                Debug.LogWarning("Sensor ] target - CharacterControl 없음");

            // 데미지 받을 수 없는 상태
            if (target.isDamageable == false)
                continue;

            Vector3 direction = (target.eyepoint.position - transform.position).normalized;

            float angle = Vector3.Angle(transform.forward, direction);
            if (angle > (fieldOfViewAngle * 0.5f))
                continue;

            currentFrameTargets.Add(target);

            // 타겟과의 거리 체크
            float distance = Vector3.Distance(transform.position, target.eyepoint.position);
            // 타겟과의 장애물 체크
            bool isVisible = !Physics.Raycast(transform.position, direction, distance, blockLayer);
            // 타겟 도착 체크
            bool isArrived = distance <= arrivedRadius;

            //현재 상태 가져오기
            visibilityStates.TryGetValue(target, out TargetState previousState);

            TargetState newState = new TargetState
            {
                isVisible = isVisible,
                isArrived = isArrived
            };

            // 새로운 타겟 출현
            if (!visibilityStates.ContainsKey(target))
            {
                visibilityStates[target] = newState;
                if(isVisible)
                    OnFound();
                else
                    OnBlocked();
                
                if (isArrived)
                    OnArrived();
            }

            // 기존 타겟의 상태 변경
            else if (previousState.isVisible != isVisible || previousState.isArrived != isArrived)
            {
                visibilityStates[target] = newState;
                if(isVisible)
                    OnFound();
                else
                    OnBlocked();

                if (isArrived && !previousState.isArrived)
                    OnArrived();
            }
        }

        // 삭제할 목록 작성
        List<CharacterControl> toRemove = new List<CharacterControl>();
        foreach (var kvp in visibilityStates)
        {
            if (!currentFrameTargets.Contains(kvp.Key))
            {
                toRemove.Add(kvp.Key);
                OnLost();
            }
        }

        // 실제 삭제
        foreach (var t in toRemove)
            visibilityStates.Remove(t);
    }

    
    void OnFound()
    {
        //owner.Display("FOUND");
        eventSensorSightEnter.from = owner;
        eventSensorSightEnter.to = target;
        eventSensorSightEnter.Raise();
    }

    void OnBlocked()
    {
        //owner.Display("BLOCKED");
        eventSensorSightExit.from = owner;
        eventSensorSightExit.to = target;
        target = null;
        eventSensorSightExit.Raise();
    }

    void OnLost()
    {        
        //owner.Display("LOST");
       
       eventSensorSightExit.from = owner;
        eventSensorSightExit.to = target;
        eventSensorSightExit.Raise();

        target = null;
    }

    void OnArrived()
    {        
        //owner.Display("ARRIVED");
        eventSensorAttackEnter.from = owner;
        eventSensorAttackEnter.to = target;
        eventSensorAttackEnter.Raise();
    }



    void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        if (visibilityStates == null)
            return;

        if (fieldOfViewAngle > 0)
        {
            Gizmos.color = Color.cyan;

            Vector3 forwardDir = transform.forward.normalized;
            Vector3 forwardEnd = transform.position + forwardDir * detectionRadius;

            Gizmos.DrawLine(transform.position, forwardEnd);

            float halfAngle = fieldOfViewAngle * 0.5f;
            Vector3 rightDir = Quaternion.AngleAxis(-halfAngle, Vector3.up) * forwardDir;
            Vector3 leftDir = Quaternion.AngleAxis(halfAngle, Vector3.up) * forwardDir;

            Vector3 leftEnd = transform.position + leftDir * detectionRadius;
            Vector3 rightEnd = transform.position + rightDir * detectionRadius;

            Gizmos.DrawLine(transform.position, leftEnd);
            Gizmos.DrawLine(transform.position, rightEnd);

            Gizmos.DrawLine(leftEnd, rightEnd);
        }

        foreach (var pair in visibilityStates)
        {
            CharacterControl target = pair.Key;
            TargetState state = pair.Value;

            if (target == null) continue;

            Gizmos.color = state.isVisible ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, target.eyepoint.position);
        }
    }
}