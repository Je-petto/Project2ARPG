using UnityEngine;
using Unity.Cinemachine;

public class GameEventListenerCameraSwitch : MonoBehaviour
{
    [SerializeField] GameEventCameraSwitch eventCameraSwitch;

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

    private void OnEventCameraSwitch(GameEventCameraSwitch e)
    {
        SwithcCamera(e.inout);
    }

    void SwithcCamera(bool on)
    {
        if(on)
            virtualcamera.Priority.Value += 1;
        else
            virtualcamera.Priority.Value -= 1;
    }


}

    // public CinemachineVirtualCameraBase cameraEvent;


    // {
    //     if (other.tag != "Player") return;

    //     var cc = other.GetComponentInParent<CharacterControl>();
    //     if (cc == null)
    //     {
    //         Debug.LogWarning($"EventCamra] 메인 카메라 없음");
    //         return;
    //     }

    //     //카메라 스위칭
    //     cc.maincamera.Priority.Value -= 1;
    //     cameraEvent.Priority.Value += 1; 
        
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     if (other.tag != "Player") return;
        
    //     var cc = other.GetComponentInParent<CharacterControl>();
    //     if (cc == null)
    //     {
    //         Debug.LogWarning($"EventCamra] 메인 카메라 없음");
    //         return;
    //     }

    //     cc.maincamera.Priority.Value += 1;
    //     cameraEvent.Priority.Value -= 1; 


    // }
