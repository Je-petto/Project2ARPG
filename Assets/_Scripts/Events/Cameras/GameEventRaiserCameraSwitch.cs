using UnityEngine;
using Unity.Cinemachine;
using System.Threading.Tasks;

public class GameEventRaiserCameraSwitch : MonoBehaviour
{

    [SerializeField] GameEventCameraSwitch eventCameraSwitch;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        SwitchCameraAsync(2000);
    }



    // void OnTriggerExit(Collider other)
    // {
    //     if (other.tag != "Player") return;

    //     eventCameraSwitch.inout = false;
    //     eventCameraSwitch?.Raise();
    // }

    async void SwitchCameraAsync(int t)
    {
        try
        {


        eventCameraSwitch.inout = true;
        eventCameraSwitch?.Raise();
        
        //1000 milliseconds = 1초
        await Task.Delay(t);

Debug.Log("2");

        eventCameraSwitch.inout = false;
        eventCameraSwitch?.Raise();
        }
        catch ( System.Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {

        }
    }


}
