using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using Project2ARPG;
using Cysharp.Threading.Tasks.Triggers;
using System.Runtime.Serialization;



// 캐릭터 컨트롤 : 허브 역할 - 캐릭터 관리
public class CharacterControl : MonoBehaviour
{
    [HideInInspector] public AbilityControl ability;
    [HideInInspector] public InputSystem_Actions actionInputs;

    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isArrived = true;

    [ReadOnly] public Rigidbody rb; //-> 메인캐릭터에 사용하기 가장 좋음
    [ReadOnly] public Animator animator;
    [ReadOnly] public Transform eyepoint;
    [ReadOnly] public Transform model;

#region Animator HashSet
    [HideInInspector] public int _MOVESPEED = Animator.StringToHash("MOVESPEED");
    [HideInInspector] public int _RUNTOSTOP = Animator.StringToHash("RUNTOSTOP");
    [HideInInspector] public int _JUMPUP = Animator.StringToHash("JUMPUP");
    [HideInInspector] public int _JUMPDOWN = Animator.StringToHash("JUMPDOWN");
    [HideInInspector] public int _SPAWN = Animator.StringToHash("SPAWN");
#endregion

    
    void Awake()
    {
        if (TryGetComponent(out ability) == false)
            Debug.LogWarning("CharacterControl ] AbilityControl 없음");

        if (TryGetComponent(out rb) == false)
            Debug.LogWarning("CharacterControl ] Nav Mesh Agent 없음");
        
        if (TryGetComponent(out animator) == false)
            Debug.LogWarning("CharacterControl ] Animator 없음");

        eyepoint = transform.Find("_EYEPOINT_");
        if (eyepoint == null)
            Debug.LogWarning("CharacterControl ] _EYEPOINT_ 없음");

        model = transform.Find("_MODEL_");
        if (model == null)
            Debug.LogWarning("CharacterControl ] _MODEL_ 없음");

        actionInputs = new InputSystem_Actions();


    }

    void OnDestroy()
    {
        actionInputs.Dispose();                              // Destroy asset object.
    }
    
    void OnEnable()
    {
        actionInputs.Enable();                                // Enable all actions within map.
    }
    
    void OnDisable()
    {
        actionInputs.Disable();                               // Disable all actions within map.
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.1f);
        
    }

    void Start()
    {
        Visable(false);
    
    }

    public void Visable(bool b)
    {
        model.gameObject.SetActive(b);

    }

    public void Animate(int hash, float duration, int layer = 0)
    {
        animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);        
    }

}





