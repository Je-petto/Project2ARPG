
using System.Threading.Tasks;
using UnityEngine;

public class EventRaiserCameraSwitch : MonoBehaviour
{

    [SerializeField] EventCameraSwitch eventCameraSwitch;


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


    // milliseconds 1000 = 1seconds ( 1000 밀리초 = 1초 )
    async void SwitchCameraAsync(int t)
    {
        try
        {
            eventCameraSwitch.inout = true;
            eventCameraSwitch?.Raise();

            await Task.Delay(t);
            
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
