using UnityEngine;
using UnityEngine.Events;

//ScriptableObject : 데이터로 사용하지만 에셋으로도 사용 가능하다
public abstract class GameEvent<T> : ScriptableObject where T : GameEvent<T>
{
    abstract public T Item { get; }

    // UnityAction은 Action에서 파생된 Unity 전용 Delegate(대리자)
    // Event를 사용하기 위한 핵심 오브젝트이다
    // 대리자는 리스트의 요소도 갖고 있다.
    public UnityAction<T> OnEventRaised;

    // 이벤트 등록
    public void Register(UnityAction<T> listener)
    {
        OnEventRaised += listener;
    }

    // 이벤트 해제
    public void Unregister(UnityAction<T> listener)
    {
        OnEventRaised -= listener;
        
    }

    public void Raise()
    {
        OnEventRaised?.Invoke(Item);

    Debug.Log($"이벤트 발동 : {Item.name}");
    }
    



}
