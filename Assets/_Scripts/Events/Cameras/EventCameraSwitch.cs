using UnityEngine;
using CustomInspector;


[CreateAssetMenu(menuName = "GameEvent/EventCameraSwitch")]
public class EventCameraSwitch : GameEvent<EventCameraSwitch>
{
    public override EventCameraSwitch Item => this;

    // 필요한 데이터 추가

    // 캐릭터 진입시 카메라 스위칭 , 퇴장시 카메라 원위치
    [ReadOnly] public bool inout;

}
