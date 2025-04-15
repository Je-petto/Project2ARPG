using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class PoolableFeedback : PoolBehaviour
{
    [SerializeField] MMF_Player feedback;
    [SerializeField] TextMeshPro textmesh;

    void Awake()
    {
    }

    void OnEnable()
    {
        feedback.RestoreInitialValues();
        feedback.ResetFeedbacks();
        feedback.PlayFeedbacks();  
    }

    void OnDisable()
    {
        Despawn();    
    }

    // 강제 비활성화 시킨다
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void SetText(string s)
    {
        textmesh.text = s;
    }
}