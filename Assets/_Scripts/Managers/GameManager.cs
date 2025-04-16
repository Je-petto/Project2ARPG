using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;
using CustomInspector;

public class GameManager : BehaviourSingleton<GameManager>
{
    [HorizontalLine("UI"),HideField] public bool _h0;
    [SerializeField] MMF_Player feedbackInformation;
    [SerializeField] TextMeshProUGUI textmeshInformation;
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
    
    protected override bool IsDontdestroy() => true;

    void Start()
    {
        ShowInfo("", 2f);
    }

    public void ShowInfo(string info, float duration = 1f)
    {
        if (feedbackInformation.IsPlaying)
            feedbackInformation.StopFeedbacks();

        // (예) 5초 동안 표시
        // 표시중에 새로운 콜
        // 1. 기존 작업 마무리하고 처리한다 => 스택 쌓아두고 처리
        // 2. 기존 작업 취소하고 새로 바로 처리한다. => 즉시 업데이트 처리
        textmeshInformation.text = info;   
        feedbackInformation.GetFeedbackOfType<MMF_Pause>().PauseDuration = duration;
        feedbackInformation.PlayFeedbacks();
    }


    
}


//유니티 (비동기 지원 안함 -> 싱글쓰레드)

//비동기 ( Async )
// 1. 코루틴 ( Co-routine ) 
// 2. Invoke 
// 3. async / await
// 4. Awaitable
// 5. CySharp - UniTask
// 6. DoTween - DoVirtual.Delay( 3f, ()=> {});


    // void OnEnable()
    // {
    //     cts?.Dispose();        
    //     cts = new CancellationTokenSource();
    // }

    // void OnDisable()
    // {
    //     cts.Cancel();
    // }

    // void OnDestroy()
    // {
    //     cts.Cancel();
    //     cts.Dispose();        
    // }

    // CancellationTokenSource cts;
    // public async UniTaskVoid DelayCallAsync(int millisec, Action oncomplete)
    // {
    //     try
    //     {
    //         await UniTask.Delay(millisec, cancellationToken:cts.Token);

    //         oncomplete?.Invoke();
    //     }
    //     catch (OperationCanceledException)
    //     {
            
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogException(e);
    //     }
    //     finally
    //     {
    //         cts.Cancel();
    //     }
    // }