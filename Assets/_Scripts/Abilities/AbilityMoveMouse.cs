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
    private float hitDistance; // hit point와 캐릭터 사이의 거리
    private ParticleSystem marker; // 마우스로 타켓 위치를 표시(3d ui)

    public AbilityMoveMouse(AbilityMoveMouseData data, CharacterControl owner) : base(data, owner)
    {
        camera = Camera.main;
        path = new NavMeshPath();
        owner.isArrived = true;

        marker = GameObject.Instantiate(data.marker).GetComponent<ParticleSystem>();
        if(marker == null)
            Debug.LogWarning("AbilityMoveMouse ] Marker - ParticleSystem 없음");

        marker.gameObject.SetActive(false);

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
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out var hit))
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
        if( corners == null || corners.Length <= 0 || owner.isArrived == true)
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
        //50을 곱한 이유 = movePerSec와 linearVelocity 값의 범위를 맞추기 위한 상수
        Vector3 movement = direction * data.movePerSec *50f * Time.deltaTime;
        owner.rb.linearVelocity = movement;
        currentVelocity = Vector3.Distance(Vector3.zero, owner.rb.linearVelocity);

        //도착 확인
        if (Vector3.Distance(nexttarget, owner.rb.position) <= data.stopdistance)
        {
            next++;

            //최종 목적지
            if ( next >= corners.Length )
            {
                owner.isArrived = true;
                owner.rb.linearVelocity = Vector3.zero;
            }
        }

        // run to stop 바깥 범위에 hit point를 찍을때마 실행

        // 최종 위치 준비 도착
        float d = Vector3.Distance(finaltarget, owner.rb.position);
        if (hitDistance > data.runtostopDistance.y &&  d <= data.runtostopDistance.x)
            {
                owner.Animate(owner._RUNTOSTOP, 0.1f); 
            }



    }

    private void MoveAnimation()
    {
        float a = owner.isArrived ? 0f : Mathf.Clamp01( currentVelocity / data.movePerSec );
        float spd = Mathf.Lerp(owner.animator.GetFloat(owner._MOVESPEED), a, Time.deltaTime * 10f);
        owner.animator.SetFloat(owner._MOVESPEED, spd);
    }


}
