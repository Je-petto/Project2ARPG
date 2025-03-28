using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using CustomInspector;
using UnityEngine.InputSystem;


// 캐릭터 컨트롤 : 허브 역할 - 캐릭터 관리
public class CharacterControl : MonoBehaviour
{
    private static readonly int moveSpeed = Animator.StringToHash("moveSpeed");
    [HideInInspector]public AbilityControl ability;
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isArrived = true;

    [ReadOnly] public Rigidbody rb; //-> 메인캐릭터에 사용하기 가장 좋음
    //[ReadOnly] public NavMeshAgent agent;
    //[ReadOnly] public CharacterController cc; -> 사용하고 싶은 방식으로 입력 후 레퍼런스 모두 교체
    [ReadOnly] public Animator animator;

    //TEMPCODE
    public CinemachineVirtualCameraBase maincamera;
    //TEMPCODE
    

    public List<AbilityData> initialAbilities;

#region Animator HashSet
    [HideInInspector] public int _MOVESPEED = Animator.StringToHash("MOVESPEED");
    [HideInInspector] public int _runtostop = Animator.StringToHash("RUNTOSTOP");
    [HideInInspector] public int _JUMPUP = Animator.StringToHash("JUMPUP");
    [HideInInspector] public int _JUMPDOWN = Animator.StringToHash("JUMPDOWN");
#endregion

    
    void Awake()
    {
        if (TryGetComponent(out ability) == false)
            Debug.LogWarning("CharacterControl ] AbilityControl 없음");

        if (TryGetComponent(out rb) == false)
            Debug.LogWarning("CharacterControl ] Nav Mesh Agent 없음");
        
        if (TryGetComponent(out animator) == false)
            Debug.LogWarning("CharacterControl ] Animator 없음");
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.1f);
        
        //InputKeyboard();
    }

    
    // void InputKeyboard()
    // {

    //     if (Input.GetButtonDown("Jump"))
    //         ability.Activate(AbilityFlag.Jump);
    // }

    void Start()
    {
        foreach( var dat in initialAbilities )
            ability.Add(dat, true);
    }

#region InputSystem

    public void OnMoveKeyBoard(InputAction.CallbackContext ctx)
    {
        if (ctx.performed || ctx.canceled)
            ability.Activate(AbilityFlag.MoveKeyboard, ctx);
    }
    
    public void OnMoveMouse(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            ability.Activate(AbilityFlag.MoveMouse, ctx);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        
        if (ctx.performed)
            ability.Activate(AbilityFlag.Jump, ctx);
    }

#endregion

}





