using System.Runtime.InteropServices;
using Unity.Cinemachine;
using UnityEngine;

public class EventCamera : MonoBehaviour
{

    public CinemachineVirtualCameraBase cameraEvent;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        var cc = other.GetComponentInParent<CharacterControl>();
        if (cc == null)
        {
            Debug.LogWarning($"EventCamra] 메인 카메라 없음");
            return;
        }

        //카메라 스위칭
        cc.maincamera.Priority.Value -= 1;
        cameraEvent.Priority.Value += 1; 
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        
        var cc = other.GetComponentInParent<CharacterControl>();
        if (cc == null)
        {
            Debug.LogWarning($"EventCamra] 메인 카메라 없음");
            return;
        }

        cc.maincamera.Priority.Value += 1;
        cameraEvent.Priority.Value -= 1; 


    }
}
