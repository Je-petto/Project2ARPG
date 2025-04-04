using UnityEngine;
using UnityEngine.InputSystem;
using CustomInspector;
using System.Collections.Generic;

public enum CursorType { MOVE, INTERACT, ATTACK, DIALOGUE}

[System.Serializable]
public class CursorData
{
    public CursorType cursorType;
    public Texture2D texture;
    public Vector2 offset;
}


public class CursorControl : MonoBehaviour
{
    [Space(20)]
    public bool IsShowDebugCursor = false;

    // eyePoint : 플레이어 눈 위치 ( 카메라 포커싱 위치 )
    // cursorPoint : 마우스 커서 위치 ( 충돌 위치 같다)
    // cursorFixedPoint : 실제 커서 위치를 캐릭터 눈높이로 고정(수정) 위치
    [Space(10)]
    [ReadOnly] public Transform eyePoint;
    
    // 충돌포인트(HitPoint) 와 같다
    [SerializeField] Transform cursorPoint; 
    // 커서의 위치를 눈높이로 수정 ( 카메라의 흔들림 방지 목적 )
    [SerializeField] Transform cursorFixedPoint;
    public Transform CursorFixedPoint => cursorFixedPoint;
    [SerializeField] LineRenderer line;
    
    private Camera cam;

    public CursorType cursorType = CursorType.MOVE;


    // 2D 커서
    [SerializeField] List<CursorData> cursors = new List<CursorData>();  


    private GameObject curHover; // 현재 커서 위치의 오브젝트
    private GameObject preHover; // 이전 커서 위치의 오브젝트

    void Start()
    {
        cam = Camera.main;
        
        line.enabled = IsShowDebugCursor;
        cursorPoint.GetComponent<MeshRenderer>().enabled = IsShowDebugCursor;
        cursorFixedPoint.GetComponent<MeshRenderer>().enabled = IsShowDebugCursor;  

        SetCursor(CursorType.MOVE);
    }

    void Update()
    {
        if (cam == null || eyePoint == null) return;

        preHover = curHover;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit))
        {
            //현재 커서 아래의 오브젝트
            curHover = hit.collider.gameObject;

            //현재 커서와 이전 커서 오브젝트가 다를때만 갱신
            if (curHover != preHover)
                OnHoverEnter();

            
            cursorPoint.position = hit.point;
            cursorFixedPoint.position = new Vector3(hit.point.x, eyePoint.position.y, hit.point.z);

            DrawLine();
        }
        else
        {
            OnHoverExit();

            curHover = null;
        }
    }

    void DrawLine()
    {
        if (IsShowDebugCursor == false) return;
            
        line.SetPosition(0, cursorPoint.position);
        line.SetPosition(1, cursorFixedPoint.position);
    }


    //커스텀 커서 적용
    public void SetCursor( CursorType type )
    {
        var cursor = cursors.Find(v => v.cursorType == type);
        if (cursor != null)
            Cursor.SetCursor(cursor.texture, cursor.offset, CursorMode.Auto);        
    }

    // 커서 안에 위치했을 때 이벤트 처리
    private void OnHoverEnter()
    {        
        if (preHover != null)
        {
            preHover.layer = LayerMask.NameToLayer("Default");
            SetCursor(CursorType.MOVE);
        }

        //Selectable 가능한 오브젝트만 커서 상호작용 한다.
        var sel = curHover.GetComponentInParent<CursorSelectable>();
        if(sel == null) return;

        //해당 오브젝트에 맞게 커서 변동          
        if (curHover != null)
        {
            curHover.layer = LayerMask.NameToLayer("Outline");
            SetCursor(sel.cursorType);
        }

    }

    // 커서를 벗어났을 때 이벤트 처리
    private void OnHoverExit()
    {
        SetCursor(CursorType.MOVE);

        if (preHover)
            preHover.layer = LayerMask.NameToLayer("Default");  

    }
}
