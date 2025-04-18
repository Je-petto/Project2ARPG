using UnityEngine;
using CustomInspector;
using System.Collections;
using UnityEngine.VFX;

public class AnimationEventListener : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    //Player
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    //Enemy
    [SerializeField] EventEnemySpawnAfter eventEnemySpawnAfter;

    // 공격
    [SerializeField] EventAttackBefore eventAttackBefore;
    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion


    [HorizontalLine("EFFECTS"),HideField] public bool _h2;
    public PoolableParticle smoke1, smoke2, swing1;
    public VisualEffect vfxSwing;  

    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h3;
    [ReadOnly] public Transform footLeft;
    [ReadOnly] public Transform footRight;
    [ReadOnly] public Transform handLeft;
    [ReadOnly] public Transform handRight;    
    
    private CharacterControl owner;
    private Transform modelRoot;



    void Awake()
    {
        if (TryGetComponent(out owner) == false)
            Debug.LogWarning("AnimationEventListener ] CharacterControl 없음");

        modelRoot = transform.FindSlot("_model_");
        if (modelRoot == null)
            Debug.LogWarning("AnimationEventListener ] modelRoot 없음");
    }

    void OnEnable()
    {
        eventPlayerSpawnAfter.Register(OneventPlayerSpawnAfter);
        eventEnemySpawnAfter.Register(OneventEnemySpawnAfter);
    }

    void OnDisable()
    {
        eventPlayerSpawnAfter.Unregister(OneventPlayerSpawnAfter);       
        eventEnemySpawnAfter.Unregister(OneventEnemySpawnAfter); 
    }

    void OneventPlayerSpawnAfter(EventPlayerSpawnAfter e)
    {
        if (owner != e.character)
            return;

        StartCoroutine(delayfind());
    }

    void OneventEnemySpawnAfter(EventEnemySpawnAfter e)
    {
        if (owner != e.character)
            return;

        StartCoroutine(delayfind());
    }

    IEnumerator delayfind()
    {   
        yield return new WaitForEndOfFrame();

        footLeft = modelRoot.FindSlot("leftfoot", "Ball_L", "l foot", "Lfoot");
        footRight = modelRoot.FindSlot("rightfoot", "Ball_R", "r foot", "Rfoot");
        handLeft = modelRoot.FindSlot("L Hand", "Hand_L", "LeftHand", "l hand");
        handRight = modelRoot.FindSlot("R Hand", "Hand_R", "RightHand", "r hand");
    }


    public void Footstep(string s)
    {        
        if (owner.isArrived == true || footLeft == null || footRight == null) return;

        PoolManager.I.Spawn(smoke1, s == "L" ? footLeft.position : footRight.position, Quaternion.identity, null);        
    }

    public void Footstop(string s)
    {
        if (footLeft == null || footRight == null) return;

        PoolManager.I.Spawn(smoke1, s == "L" ? footLeft.position : footRight.position, Quaternion.identity, null);
    }

    public void Jumpdown()
    {
        Vector3 offset = Vector3.up * 0.1f + Random.insideUnitSphere * 0.2f;
        PoolManager.I.Spawn(smoke2, owner.model.position + offset, Quaternion.identity, null);
    }

    //Swing L : left, R : right
    public void Attack(string s)
    {
        //해당 프레임에서 공격 이벤트 발생 : 가해자 = owner
        eventAttackBefore.from = owner;
        eventAttackBefore.Raise();
        
        //확률 30~50%
        int rnd = Random.Range(0, 3);
        if (rnd < 1) return;
        
        var rot = Quaternion.LookRotation(owner.transform.forward, Vector3.up);
        rot.eulerAngles = new Vector3(-90f, rot.eulerAngles.y, 0f);
        PoolManager.I.Spawn(swing1, s == "L" ? handLeft.position : handRight.position, rot, null);        
    }

    // string : on : 이펙트 시작/ off: 이펙트 오프
    // Visual Effect 사용
    public void Swing(string on)
    {
        owner.feedback.PlayerSwingTrail(on.ToLower() == "on");

    }
}
