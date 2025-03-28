using UnityEngine;
using UnityEngine.InputSystem;

public class CursorControl : MonoBehaviour
{
    [Space(20)]
    public bool IsShow = false;
    
    //eyePoint : 플레이어 눈 위치 (카메라 포커싱 위치)
    //hitPoint : 마우스와 레벨 충돌 위치
    //cursorPoint : 마우스와 레벨 충돌 위치를 플레이어에게 알려줌
    [Space(10), SerializeField] Transform eyePoint;
    [SerializeField] Transform hitPoint;
    [SerializeField] Transform cursorPoint;
    [SerializeField] LineRenderer line;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (IsShow)
        {
            
            line.enabled = IsShow;
            hitPoint.GetComponent<MeshRenderer>().enabled = IsShow;
            cursorPoint.GetComponent<MeshRenderer>().enabled =IsShow;
        }
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out var hit))
        {
            hitPoint.position = hit.point;
            cursorPoint.position = new Vector3(hit.point.x, eyePoint.position.y, hit.point.z);

            DrawLine();
        }
    }

    void DrawLine()
    {
        line.SetPosition(0, hitPoint.position);
        line.SetPosition(1, cursorPoint.position);
    }
}
