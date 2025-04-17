using UnityEngine;
using UnityEngine.AI;


public class AbilityTrace : Ability<AbilityTraceData>
{
    private NavMeshPath path;
    private Vector3[] corners;
    private int next;
    
    private float currentVelocity;



    public AbilityTrace(AbilityTraceData data, CharacterControl owner) : base(data,owner)
    {        
        if (owner.Profile == null) return;

        path = new NavMeshPath();
        owner.isArrived = true;
        
        data.movePerSec = owner.Profile.movespeed;
        data.rotatePerSec = owner.Profile.rotatespeed;        
    }

    public override void Activate(object obj)
    {
        //obj 는 추격 대상 (Target)
        data.target = obj as CharacterControl;
        if (data.target == null)
            return;

        owner.ui.Display(data.Flag.ToString());
    }

    public override void Deactivate()
    {        
        owner.Stop();   
    }

    public override void Update()
    {
        TargetPosition();
        MoveAnimation();
    }

    public override void FixedUpdate()
    {
        if ( owner == null || owner.rb == null)
            return;

        FollowPath();
    }
    

    // 추격 대상을 정한다.
    void TargetPosition()
    {        
        if (data.target == null || owner.isArrived == false)
            return;            

        SetDestination(data.target.transform.position);
    }


    private void SetDestination(Vector3 destination)
    {
        if (NavMesh.CalculatePath(owner.transform.position, destination, -1, path) == false)
            return;

        corners = path.corners;
        next = 1;
        owner.isArrived = false;     
    }

    
    Quaternion _lookrot;
    private void FollowPath()
    {

        if ( corners == null || corners.Length <= 0 || owner.isArrived == true)
            return;

        // 다음 위치
        Vector3 nexttarget = corners[next];
        // 최종 위치
        //Vector3 finaltarget = corners[corners.Length-1];
        // 다음 위치 방향
        Vector3 direction = (nexttarget - owner.transform.position).normalized;
        direction.y = 0;


        //회전
        if (direction != Vector3.zero)
            _lookrot = Quaternion.LookRotation(direction);

        owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, _lookrot, data.rotatePerSec * Time.deltaTime);

        //이동
        // 50 곱한 이유 : movePerSec 과 linearVelocity 값을 동기화 위한 상수
        Vector3 movement = direction * data.movePerSec * 50f * Time.deltaTime;        
        owner.rb.linearVelocity = movement;
        currentVelocity = Vector3.Distance(Vector3.zero, owner.rb.linearVelocity);

    
        //도착 확인
        if (Vector3.Distance(nexttarget, owner.rb.position) <= data.stopDistance)
        {
            next++;

            // 최종 목적지 도착
            if ( next >= corners.Length )
                owner.Stop();
        }        
    }

    private void MoveAnimation()
    {
        float a = owner.isArrived ? 0f : Mathf.Clamp01(currentVelocity / data.movePerSec);
        owner.AnimateMovespeed(a, false);
    }


    
}
