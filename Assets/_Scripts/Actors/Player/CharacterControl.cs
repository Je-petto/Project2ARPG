using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using Project2ARPG;


// instance , copy
// 연결(ref), 독립
// 캐릭터 관리 ( 허브 )
public class CharacterControl : MonoBehaviour, IActorControl 
{

    public ActorProfile Profile{ get => profile; set => profile = value; }    
    [ReadOnly, SerializeField] private ActorProfile profile;
    
    [HideInInspector] public AbilityControl ability;
    [HideInInspector] public InputSystem_Actions actionInputs;

    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isArrived = true;
    
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;
    [ReadOnly] public Transform eyepoint;
    [ReadOnly] public Transform model;



    void Awake()
    {
        actionInputs = new InputSystem_Actions();

        if (TryGetComponent(out ability) == false)
            Debug.LogWarning("CharacterControl ] AbilityControl 없음");

        if (TryGetComponent(out rb) == false)
            Debug.LogWarning("CharacterControl ] Rigidbody 없음");

        if (TryGetComponent(out animator) == false)
            Debug.LogWarning("CharacterControl ] Animator 없음");

        eyepoint = transform.Find("_EYEPOINT_");
        if (eyepoint == null)
            Debug.LogWarning("CharacterControl ] _EYEPOINT_ 없음");

        model = transform.Find("_MODEL_");
        if (model == null)
            Debug.LogWarning("CharacterControl ] _MODEL_ 없음");
    }


    void OnDestroy()
    {
        actionInputs.Dispose();
    }
    
    void OnEnable()
    {
        actionInputs.Enable();
    }
    
    void OnDisable()
    {
        actionInputs.Disable();
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.1f);
    }

    public void Visible(bool b)
    {
        model.gameObject.SetActive(b);
    }

    public void Animate(int hash, float duration, int layer = 0)
    {
        animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);
    }


}