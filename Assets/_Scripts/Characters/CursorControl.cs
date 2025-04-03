using CustomInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public enum CursorType { MOVE, ATTACK, INTERACT, DIALOGUE}

[System.Serializable]
public class CursorData
{
    public CursorType type;
    public Texture2D texture;
    public Vector2 offset;

}

public class CursorControl : MonoBehaviour
{
    [Space(20)]
    public bool IsShowDebugCursor = false;
    
    //eyePoint : 플레이어 눈 위치 (카메라 포커싱 위치)
    //cursorPoint : 마우스 커서 위치 (충돌 위치와 같다)
    //cursorFixedPoint : 실제 커서 위치를 캐릭터 눈높이로 고정(수정) 위치
    [Space(10)]
    [ReadOnly] public Transform eyePoint;
    public Transform EyePoint { get => eyePoint; set => eyePoint = value; }
    
    // 충돌포인트(HitPoint하고 같다)
    [SerializeField] Transform cursorPoint;
    // 커서의 위치를 눈높이로 tnwjd(카메라 흔들림 방지 목적)
    [SerializeField] Transform cursorFixedPoint;
    public Transform CursorFixedPoint => cursorFixedPoint;
    [SerializeField] LineRenderer line;

    private Camera cam;
    public CursorType cursorType = CursorType.MOVE;


    // 2D 커서
    [SerializeField] List<CursorData> cursors = new List<CursorData>();


    void Start()
    {
        cam = Camera.main;

            line.enabled = IsShowDebugCursor;
            cursorPoint.GetComponent<MeshRenderer>().enabled = IsShowDebugCursor;
            cursorFixedPoint.GetComponent<MeshRenderer>().enabled =IsShowDebugCursor;


        SetCursor(cursorType);
    }

    void Update()
    {
        if (cam == null || eyePoint == null ) return;
        
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out var hit))
        {
            cursorPoint.position = hit.point;
            cursorFixedPoint.position = new Vector3(hit.point.x, eyePoint.position.y, hit.point.z);

            DrawLine();
        }
    }

    void DrawLine()
    {
        if (IsShowDebugCursor == false) return;

        line.SetPosition(0, cursorPoint.position);
        line.SetPosition(1, cursorFixedPoint.position);
    }

        // 커스텀 커서 적용
    public void SetCursor( CursorType type)
    {
        var cursor = cursors.Find( v => v.type == type);
        if (cursor != null)
            Cursor.SetCursor(cursor.texture, cursor.offset, CursorMode.Auto);
    }
}
