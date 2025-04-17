using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using UnityEngine;

public class FeedbackControl : MonoBehaviour
{
    [SerializeField] MMF_Player feedbackImpact;
    [SerializeField] MMF_Player feedbackSwingTrail;

    //초기 설정
    void Start()
    {
        //VFX 끄고 시작
        PlayerSwingTrail(false);
    }

    
    //피격 시 반짝임 효과
    public void PlayImpact()
    {
        feedbackImpact?.PlayFeedbacks();
    }

    //스윙 시 트레일 효과 (Visual Effect를 사용, Play와 Stop를 토글한다)
    public void PlayerSwingTrail(bool on)
    {
        if (feedbackSwingTrail == null)
            return;

        var swing = feedbackSwingTrail.GetFeedbackOfType<MMF_VisualEffect>();
        if (swing == null)
            return;
        
        if (on)
            swing.Mode = on ? MMF_VisualEffect.Modes.Play : MMF_VisualEffect.Modes.Stop;

        feedbackSwingTrail.PlayFeedbacks();
    }
}
