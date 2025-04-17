
using UnityEngine;
using CustomInspector;
using DG.Tweening;



//TEMPCODE
public struct CharacterState
{
    // 현재 체력 (HP)
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
    public ActorProfile Profile { get => profile; set => profile = value; }    
    // 원본 데이터
    [ReadOnly, SerializeField] private ActorProfile profile;
    // 인스턴스화 한 데이터
    public CharacterState State;


    [ReadOnly] public AbilityControl ability;
    [ReadOnly] public UiControl ui;
    [ReadOnly] public FeedbackControl feedback;
    

    // 땅에 붙어 있나 ? TRUE : 땅에 붙었다
    [ReadOnly] public bool isGrounded;
    // 데미지 받을 수 있는 상태인가 ? TRUE : 데미지 가능
    [ReadOnly] public bool isDamageable = false;
    // 목적지 도착 했나 ? TRUE : 도착했다
    [ReadOnly] public bool isArrived = true;
    
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;
    [ReadOnly] public AnimationIKControl ik;
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
            Debug.LogWarning("CharacterControl ] UiControl 없음");
      


        eyepoint = transform.Find("_EYEPOINT_");
        if (eyepoint == null)
            Debug.LogWarning("CharacterControl ] _EYEPOINT_ 없음");

        model = transform.Find("_MODEL_");
        if (model == null)
            Debug.LogWarning("CharacterControl ] _MODEL_ 없음");        


        // 옵션 : 있으면 쓰고 , 없으면 안쓴다
        ik = GetComponent<AnimationIKControl>();
    }


    

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.1f);        
    }


    Tween tweenrot;
    // 타겟을 바라본다 (Y축만 회전)
    public void LookatY(Vector3 target)
    {
        if (tweenrot != null)
            tweenrot.Kill(true);

        target.y = 0;

        Vector3 direction = target - new Vector3(eyepoint.position.x, 0f, eyepoint.position.z);        
        Quaternion rot = Quaternion.LookRotation(direction.normalized);        

        tweenrot = transform.DORotateQuaternion(rot, 0.2f).SetEase(Ease.OutSine);        
    }

    // 이동 멈춤
    public void Stop()
    {
        isArrived = true;
        rb.linearVelocity = Vector3.zero;
    }

    public void Visible(bool b)
    {
        model.gameObject.SetActive(b);
    }



#region ANIMATE

    // 단순한 애니메이터 해시 플레이
    public void Animate(string key, float duration, int layer = 0)
    {
        animator?.CrossFadeInFixedTime(key, duration, layer, 0f);
    }

    // 다양한 모션을 가져와서 애니메이터 Name 플레이
    public void Animate(string key, AnimatorOverrideController aoc, AnimationClip clip, float duration, int layer)
    {
        if (animator == null) return;

        aoc[key] = clip;
        animator.runtimeAnimatorController = aoc;        
        animator.CrossFadeInFixedTime(key, duration, layer, 0f);
    }


    // Trigger 는 일회성 호출
    public void AnimateTrigger(string key, AnimatorOverrideController aoc, AnimationClip clip)
    {
        if (animator == null) return;

        aoc[key] = clip;
        animator.runtimeAnimatorController = aoc;
        animator.SetTrigger(key);
    }



    // immediate TRUE : 보간처리 없이 바로 목표 값으로 애니메이션
    public void AnimateMovespeed(float targetspeed, bool immediate)
    {
        if (animator == null) return;

        float cur = animator.GetFloat("MOVESPEED");
        float spd = Mathf.Lerp(cur, targetspeed, Time.deltaTime * 10f);
        animator.SetFloat("MOVESPEED", immediate ? targetspeed : spd);
    }
    
#endregion

}