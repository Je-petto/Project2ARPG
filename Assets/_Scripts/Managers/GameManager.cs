using UnityEngine.Events;

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

}
