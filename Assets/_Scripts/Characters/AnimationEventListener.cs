using UnityEngine;
using CustomInspector;
using System.Collections;

public class AnimationEventListener : MonoBehaviour
{

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion


    public PoolableParticle smoke1, smoke2;  

    [ReadOnly] public Transform footLeft;
    [ReadOnly] public Transform footRight;
    
    private CharacterControl cc;
    private Transform modelRoot;



    void Awake()
    {
        if (TryGetComponent(out cc) == false)
            Debug.LogWarning("AnimationEventListener ] CharacterControl 없음");

        modelRoot = transform.FindSlot("_model_");
        if (modelRoot == null)
            Debug.LogWarning("AnimationEventListener ] modelRoot 없음");
    }

    void OnEnable()
    {
        eventPlayerSpawnAfter.Register(OneventPlayerSpawnAfter);
    }

    void OnDisable()
    {
        eventPlayerSpawnAfter.Unregister(OneventPlayerSpawnAfter);        
    }


    void OneventPlayerSpawnAfter(EventPlayerSpawnAfter e)
    {
        if ( cc.actorType != e.actorProfile.type )
            return;
            
        StartCoroutine(delayfind());
    }

    IEnumerator delayfind()
    {   
        yield return new WaitForEndOfFrame();

        footLeft = modelRoot.FindSlot("leftfoot");
        footRight = modelRoot.FindSlot("rightfoot");
    }

    

    public void Footstep(string s)
    {        
        if (cc.isArrived == true || footLeft == null || footRight == null) return;

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
        PoolManager.I.Spawn(smoke2, cc.model.position + offset, Quaternion.identity, null);
    }
}
