using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;



// 관리 , 이벤트 송출(SEND)

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
        catch (OperationCanceledException)
        {
            
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            cts.Cancel();
        }
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
