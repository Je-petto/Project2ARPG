using UnityEngine;
using CustomInspector;
using TMPro;



// instance , copy
// 연결(ref), 독립
// 캐릭터 관리 ( 허브 )
public class CharacterControl : MonoBehaviour
{

    public ActorProfile Profile{ get => profile; set => profile = value; }    
    [ReadOnly, SerializeField] private ActorProfile profile;
    
    [HideInInspector] public AbilityControl ability;

    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isArrived = true;
    
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;
    [ReadOnly] public Transform eyepoint;
    [ReadOnly] public Transform model;
    [ReadOnly] public TextMeshPro uiInfo;



    void Awake()
    {

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

        uiInfo = GetComponentInChildren<TextMeshPro>();
    }


    void Update()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.1f);
    }

    // 타겟을 바라본다 (Y 축만 회전)
    public void LookatY(Vector3 target)
    {
        target.y = 0;
        
        Vector3 direction = target - new Vector3(eyepoint.position.x, 0f, eyepoint.position.z);
        Vector3 eular = Quaternion.LookRotation(direction.normalized).eulerAngles;
        transform.rotation = Quaternion.Euler(eular);        
    }

    public void Stop()
    {
        isArrived = true;
        rb.linearVelocity = Vector3.zero;
    }

    public void Visible(bool b)
    {
        model.gameObject.SetActive(b);
    }

    public void Animate(int hash, float duration, int layer = 0)
    {
        animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);
    }

    public void Display(string info)
    {
        if(uiInfo == null) return;

        uiInfo.text = info;

    }

#region ANIMATE   
    public void AnimateMoveSpeed(float targetspeed)
    {
        if (animator == null) return;
        
        float cur = animator.GetFloat(AnimatorHashes._MOVESPEED);
        float spd = Mathf.Lerp(cur, targetspeed, Time.deltaTime * 10f);
        animator.SetFloat(AnimatorHashes._MOVESPEED, spd);
    }

    public void AnimateOnceAttack(Vector3 target)
    {
        LookatY(target);
        Animate(AnimatorHashes._ATTACK, 0.1f);        
    }
#endregion
}