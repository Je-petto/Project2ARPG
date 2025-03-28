using UnityEngine.Events;

//Send
public class GameEvent : BehaviourSingleton<GameEvent>
{

    protected override bool IsDontdestroy() => true;

}
