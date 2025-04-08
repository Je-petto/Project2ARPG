using UnityEngine;
using UnityEngine.AI;

public class AbilityWander : Ability<AbilityWanderData>
{
    private NavMeshPath path;
    private Vector3[] corners;
    private int next;
    
    private float currentVelocity;

    public AbilityWander(AbilityWanderData data, CharacterControl owner) : base(data,owner)
    {
        
        if (owner.Profile == null) return;

        path = new NavMeshPath();
        owner.isArrived = true;

        //프로파일 속성 연결
        if (owner.Profile == null) return;

        data.movePerSec = owner.Profile.movespeed;
        data.rotatePerSec = owner.Profile.rotatespeed;

    }

    public override void Activate()
    {

        
    }
    public override void Deactivate()
    {


    }

    float elapsed;
    public override void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > data.wanderStay)
        {
            RandomPosition();
            elapsed = 0f;

        }
        
        MoveAnimation();
    }

    public override void FixedUpdate()
    {
        if ( owner == null || owner.rb == null)
            return;

        FollowPath();
    }
    
    // 이동할 랜덤 위치를 정한다.
    void RandomPosition()
    {

        // isArrived == false : 가는 중이다
        if (owner.isArrived == false)
            return;
        
        Vector3 rndpos = owner.transform.position + Random.insideUnitSphere * data.wanderRadius;
        rndpos.y = 1f;

        SetDestination(rndpos);

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
        Vector3 finaltarget = corners[corners.Length-1];
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
            {
                owner.isArrived = true;
                owner.rb.linearVelocity = Vector3.zero;
            }
        }
        
    }

    private void MoveAnimation()
    {   
        float a = owner.isArrived ? 0f : Mathf.Clamp01(currentVelocity / data.movePerSec);
        float spd = Mathf.Lerp(owner.animator.GetFloat(AnimatorHashes._MOVESPEED), a, Time.deltaTime * 10f);
        owner.animator.SetFloat(AnimatorHashes._MOVESPEED, spd);
    }


}
