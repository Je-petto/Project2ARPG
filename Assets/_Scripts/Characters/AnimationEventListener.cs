using UnityEngine;
using CustomInspector;
using System.Collections;

public class AnimationEventListener : MonoBehaviour
{
    public PoolableParticle smoke1, smoke2;

    [ReadOnly] public Transform footLeft;
    [ReadOnly] public Transform footRight;

    private CharacterControl cc;
    private Transform modelRoot;

#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;
    [SerializeField] EventCameraSwitch eventCameraSwitch;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;

#endregion

    void Awake()
    {
        if (TryGetComponent(out cc) == false)
            Debug.LogWarning("AnimationEventListener ] CharacterControl 없음");
    }

    void OnEnable()
    {

        eventPlayerSpawnAfter.Register(OneventPlayerSpawnAfter);

    }

    void OnDisable()
    {
        eventPlayerSpawnAfter.Unregister(OneventPlayerSpawnAfter);

    }

    //주기적으로 검사
    void OneventPlayerSpawnAfter(EventPlayerSpawnAfter e)
    {
        modelRoot = transform.FindSlot("_model_");
        if (modelRoot == null)
            Debug.LogWarning("AnimationEventListener ] ModelRoot 없음");

        
        StartCoroutine(delayfind());
        // modelRoot.FindSlot("leftfoot");
        // modelRoot.FindSlot("rightfoot");        
    }

    IEnumerator delayfind()
    {
        modelRoot = transform.FindSlot("_model_");
        if (modelRoot == null)
            Debug.LogWarning("AnimationEventListener ] modelRoot 없음");

        yield return new WaitForSeconds(1f);
        
        footLeft = modelRoot.FindSlot("leftfoot");
        footRight = modelRoot.FindSlot("rightfoot");
    }

}
