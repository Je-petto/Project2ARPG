using UnityEngine;
using UnityEngine.Events;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

//Send
public class GameManager : BehaviourSingleton<GameManager>
{
    protected override bool IsDontdestroy() => true;

#region 이벤트 선언
    public UnityAction eventTestEvent;
#endregion

#region 이벤트 호출
    public void TriggerCameraEvent() => eventTestEvent?.Invoke();
#endregion


    void OnEnable()
    {

        cts?.Dispose();
        cts = new CancellationTokenSource();
    }

    void OnDisable()
    {
        cts.Cancel();        
    }

    void OnDestroy()
    {
        cts.Cancel();
        cts.Dispose();        
    }

    CancellationTokenSource cts;
    public async UniTaskVoid DelayCallAsync(int millisec, Action oncomplete)
    {
        try
        {
            await UniTask.Delay(1000, cancellationToken:cts.Token);

            oncomplete?.Invoke();

        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            cts.Cancel();
        }
    }
}
