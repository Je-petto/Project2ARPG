using UnityEngine;
using Unity.Cinemachine;


public class EventListenerCameraSwitch : MonoBehaviour
{
    [SerializeField] EventCameraSwitch eventCameraSwitch;


    [SerializeField] CinemachineVirtualCameraBase virtualcamera;

    void Start()
    {
        virtualcamera.Priority = 0;
    }

    void OnEnable()
    {
        eventCameraSwitch.Register(OnEventCameraSwitch);        
    }

    void OnDisable()
    {
        eventCameraSwitch.Unregister(OnEventCameraSwitch);        
    }

    private void OnEventCameraSwitch(EventCameraSwitch e)
    {
        SwitchCamera(e.inout);
    }


    void SwitchCamera(bool on)
    {
        if (on)
            virtualcamera.Priority.Value += 1;
        else
            virtualcamera.Priority.Value -= 1;
    }

}

