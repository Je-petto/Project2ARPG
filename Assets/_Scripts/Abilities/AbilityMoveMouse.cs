using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class AbilityMoveMouse : Ability<AbilityMoveMouseData>
{
    private Camera camera;
    private NavMeshPath path;
    private Vector3[] corners;
    private int next;
    
    private float currentVelocity;
    private float hitDistance; // hit point 와 캐릭터 간의 거리

    private ParticleSystem marker; // 마우스로 타겟 위치를 표시 (3D UI)

    

    public AbilityMoveMouse(AbilityMoveMouseData data, CharacterControl owner) : base(data, owner)
    {
        camera = Camera.main; 
        path = new NavMeshPath();
        owner.isArrived = true;

        marker = GameObject.Instantiate(data.marker);
        if (marker == null)
            Debug.LogWarning("AbilityMoveMouse ] Marker - ParticleSystem 없음");
        
        marker.gameObject.SetActive(false);

        //프로파일 속성 연결
        if (owner.profile == null) return;

        data.movePerSec = owner.profile.movespeed;
        data.rotatePerSec = owner.profile.rotatespeed;
    }



    public override void Update()
    {
        if ( owner == null || owner.rb == null)
            return;

        MoveAnimation();
    }

    public override void FixedUpdate()
    {
        if ( owner == null || owner.rb == null)
            return;

        FollowPath();
    }


    public override void Activate()
    {
        owner.actionInputs.Player.MoveMouse.performed += InputMove;
    }

    public override void Deactivate()
    {
        owner.actionInputs.Player.MoveMouse.performed -= InputMove;
    }


    private void InputMove(InputAction.CallbackContext ctx)
    {
        // 마우스움직임(2D좌표) -> 카메라 Ray -> hit.point(3D좌표)
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit))
        {
            marker.gameObject.SetActive(true);
            marker.transform.position = hit.point + Vector3.up * 0.1f;
            marker.Play();

            hitDistance = Vector3.Distance(hit.point, owner.rb.position);
            SetDestination(hit.point);
        }
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


        // run to stop 바깥 범위에 hit point 를 찍을때만 실행        
        //최종 위치 준비 동작
        float d = Vector3.Distance(finaltarget, owner.rb.position);
        if (hitDistance > data.runtostopDistance.y && d <= data.runtostopDistance.x)
        {
            owner.Animate(owner._RUNTOSTOP, 0.1f);
        }
        
    }

    private void MoveAnimation()
    {
        float a = owner.isArrived ? 0f : Mathf.Clamp01(currentVelocity / data.movePerSec);
        float spd = Mathf.Lerp(owner.animator.GetFloat(owner._MOVESPEED), a, Time.deltaTime * 10f);
        owner.animator.SetFloat(owner._MOVESPEED, spd);
    }

}
