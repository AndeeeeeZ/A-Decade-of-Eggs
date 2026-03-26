using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RestartHelper : MonoBehaviour
{
    public UnityEvent Restart; 
    private GameInputActions inputs; 

    private void Awake()
    {
        inputs = new GameInputActions(); 
    }

    private void OnEnable()
    {
        inputs.Enable(); 
        inputs.Menu.Restart.performed += OnRestart; 
    }

    private void OnDisable()
    {
        inputs.Menu.Restart.performed -= OnRestart;
        inputs.Disable(); 
    }

    private void OnRestart(InputAction.CallbackContext contest)
    {
        Restart?.Invoke(); 
    }

    public void ToRestart()
    {
        Restart?.Invoke(); 
    }
}
