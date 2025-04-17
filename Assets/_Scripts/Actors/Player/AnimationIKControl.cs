using UnityEngine;
using CustomInspector;
using DungeonArchitect.Flow.Exec;

public class AnimationIKControl : MonoBehaviour
{

    [HorizontalLine("Targets"),HideField] public bool _h0;

    [Space(10)]
    public bool isTarget = false;
    [Tooltip("바라볼 타겟 (Transform)")]
    public Transform target;


    [HorizontalLine("IKSettings"),HideField] public bool _h1;

    [Tooltip("전체 IK 수치")]
    [Range(0f, 1f), SerializeField] float overallWeight = 1f;

    [Tooltip("몸통(Spine부터...Neck까지) IK 수치")]
    [Range(0f, 1f), SerializeField] float bodyWeight = 0.5f;

    [Tooltip("머리(Head) IK 수치")]
    [Range(0f, 1f), SerializeField] float headWeight = 0.8f;

    private Animator animator;
    private float currentWeight;


    void Start()
    {
        if (TryGetComponent(out animator) == false)
            Debug.LogWarning("AnimationIKControl ] Animator 없음");
    }

    void Update()
    {
        if (animator == null || target == null)
            return;

        // 타겟 있으면, overall 값 , 없으면 0
        float targetWeight = (isTarget && target != null) ? overallWeight : 0f;

        currentWeight = Mathf.MoveTowards(currentWeight, targetWeight, Time.deltaTime * 10f);   

    }

    // Unity 예약함수 - IK 연산처리
    void OnAnimatorIK(int layerIndex)
    {
        if (target == null || currentWeight <= 0.01f)
        {
            animator.SetLookAtWeight(0f);
            return;
        }
        animator.SetLookAtWeight(overallWeight, bodyWeight, headWeight);
        animator.SetLookAtPosition(target.position);
    }

}
