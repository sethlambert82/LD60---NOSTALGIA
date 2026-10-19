using UnityEngine;

public class InputLoader : MonoBehaviour
{
    
    public static InputSystem_Actions controls;

    public void OnEnable()
    {
        if(controls==null)
        {
            controls = new InputSystem_Actions();
        }
        controls.Enable();
    }

    public void OnDisable()
    {
        controls.Disable();
    }
}
