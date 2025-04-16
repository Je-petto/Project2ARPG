using UnityEngine;
using CustomInspector;


//TEMPCODE
public struct CharactertState
{
    // 현재 체력()
    public int health;

    // 현재 공격력
    public int damage;



    public void Set(ActorProfile profile)
    {
        health = profile.health;
        damage = profile.attackdamage;
    }
}
//TEMPCODE

// instance , copy
// 연결(ref), 독립
// 캐릭터 관리 ( 허브 )
public class CharacterControl : MonoBehaviour
{

    public ActorProfile Profile{ get => profile; set => profile = value; }    
    
    // 원본 데이터
    [ReadOnly, SerializeField] private ActorProfile profile;
    // 인스턴스화 한 데이터
    public CharactertState State;
    
    [HideInInspector] public AbilityControl ability;
    [ReadOnly] public UIControl ui;

    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isArrived = true;
    
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;
    [ReadOnly] public Transform eyepoint;
    [ReadOnly] public Transform model;




    void Awake()
    {

        if (TryGetComponent(out ability) == false)
            Debug.LogWarning("CharacterControl ] AbilityControl 없음");

        if (TryGetComponent(out rb) == false)
            Debug.LogWarning("CharacterControl ] Rigidbody 없음");

        if (TryGetComponent(out animator) == false)
            Debug.LogWarning("CharacterControl ] Animator 없음");

        if (TryGetComponent(out ui) == false)
            Debug.LogWarning("CharactorControl ] UIControl 없음");

        eyepoint = transform.Find("_EYEPOINT_");
        if (eyepoint == null)
            Debug.LogWarning("CharacterControl ] _EYEPOINT_ 없음");

        model = transform.Find("_MODEL_");
        if (model == null)
            Debug.LogWarning("CharacterControl ] _MODEL_ 없음");
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

    // 단순한 애니메이터 해시 플레이
    public void Animate(int hash, float duration, int layer = 0)
    {
        animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);
    }

    // 다양한 모션을 가져와서 애니메이터 Name 플레이
    public void Animate(string name, AnimatorOverrideController aoc, AnimationClip clip, float anispd, float duration, int layer = 0)
    {
        if (animator == null) return;

        // 애니메이션 속도와 클립 길이를 반영한 결과를 만든다.
        float d = 1f / anispd;
        float s = clip.length / d;

        aoc[name] = clip;
        animator.runtimeAnimatorController = aoc;
        animator.CrossFadeInFixedTime(name, duration, layer, 0f);
    }
    

#region ANIMATE   
    //immediate TRUE : 보간처리 없이 바로 목표 값으로 애니메이션
    public void AnimateMoveSpeed(float targetspeed,  bool immediate)
    {
        if (animator == null) return;
        
        float cur = animator.GetFloat(AnimatorHashes._MOVESPEED);
        float spd = Mathf.Lerp(cur, targetspeed, Time.deltaTime * 10f);
        animator.SetFloat(AnimatorHashes._MOVESPEED, immediate ? targetspeed : spd);
    }

    public void AnimateOnceAttack(Vector3 target)
    {
        LookatY(target);
        Animate(AnimatorHashes._ATTACK, 0.1f);        
    }
#endregion
}