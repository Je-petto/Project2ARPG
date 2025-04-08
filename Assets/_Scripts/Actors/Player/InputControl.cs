using UnityEngine;
using Project2ARPG;

public class InputControl : MonoBehaviour
{
    [HideInInspector] public InputSystem_Actions actionInputs;

    void Awake()
    {
        actionInputs = new InputSystem_Actions();
    }
    
    void OnDestroy()
    {
        actionInputs.Dispose();
    }
    
    void OnEnable()
    {
        actionInputs.Enable();
    }
    
    void OnDisable()
    {
        actionInputs.Disable();
    }

}
