using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

//Send
public class GameManager : BehaviourSingleton<GameManager>
{
    protected override bool IsDontdestroy() => true;

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
            await UniTask.Delay(millisec, cancellationToken:cts.Token);

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
