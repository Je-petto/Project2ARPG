using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameEvent<T> : ScriptableObject where T : GameEvent<T>
{
    abstract public T Item { get; }

    // UnityAction 은 Action 에서 파생 된 Unity 전용 Delegate(대리자)
    // 이벤트를 사용하기위한 핵심 오브젝트 이다
    // 대리자 는 리스트의 요소도 갖고있다.
    public UnityAction<T> OnEventRaised;

    // 이벤트 등록
    public void Register( UnityAction<T> listener )
    {        
        OnEventRaised += listener;   
    }

    // 이벤트 해제
    public void Unregister( UnityAction<T> listener )
    {
        OnEventRaised -= listener;
    }
    
    // 이벤트 발동(사용)
    public void Raise()
    {
        OnEventRaised?.Invoke(Item);
    }
}
