using UnityEngine;
using Project2ARPG;

public class InputControl : MonoBehaviour
{
    [HideInInspector] public InputSystem_Actions  actionInput;

    void Awake()
    {
        actionInput = new InputSystem_Actions();
    }
    
    void OnDestroy()
    {
        actionInput.Dispose();
    }
    
    void OnEnable()
    {
        actionInput.Enable();
    }
    
    void OnDisable()
    {
        actionInput.Disable();
    }

}
