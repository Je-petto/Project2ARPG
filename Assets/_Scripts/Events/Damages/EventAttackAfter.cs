using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventAttackAfter")]
public class EventAttackAfter : GameEvent<EventAttackAfter>
{
    //F2 누루면 다른 연결된 script에서도 이름 바뀜
    //Ctrl + H 누루면 goekd script에서만 이름 바뀜
    public override EventAttackAfter Item => this;

    // From (가해자)
    [ReadOnly] public CharacterControl from;

    // To (피해자)
    [ReadOnly] public CharacterControl to;

    // Damage (피해량)
    [ReadOnly] public int damage;

    [Tooltip("타격시 파티클")]
    public PoolableParticle particleHit;
    
    [Tooltip("타격시 데미지 수치")]
    public PoolableFeedback feedbackFloatingText;
}
