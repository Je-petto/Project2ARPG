using UnityEngine;
using CustomInspector;


public class EnemyControl : MonoBehaviour, IActorControl
{


    public ActorProfile Profile{ get => profile; set => profile = value; }    
    [ReadOnly, SerializeField] private ActorProfile profile;


    [HideInInspector] public AbilityControl ability;
    
    [ReadOnly] public bool isArrived = true;
    
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;
    //시네마틱 연출을 위한 eyepoint
    [ReadOnly] public Transform eyepoint;
    [ReadOnly] public Transform model;



#region Animator HashSet

#endregion


    void Awake()
    {

        if (TryGetComponent(out ability) == false)
            Debug.LogWarning("EnemyControl ] AbilityControl 없음");

        if (TryGetComponent(out rb) == false)
            Debug.LogWarning("EnemyControl ] Rigidbody 없음");

        if (TryGetComponent(out animator) == false)
            Debug.LogWarning("EnemyControl ] Animator 없음");

        eyepoint = transform.Find("_EYEPOINT_");
        if (eyepoint == null)
            Debug.LogWarning("EnemyControl ] _EYEPOINT_ 없음");

        model = transform.Find("_MODEL_");
        if (model == null)
            Debug.LogWarning("EnemyControl ] _MODEL_ 없음");
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