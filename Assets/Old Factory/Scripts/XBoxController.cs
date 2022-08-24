using System;
using Core;
using UnityEngine;

public class XBoxController : MonoBehaviour,  IInputDevice //doesn't need to be a monobehaviour as it's created in the factory hub
{
    private Vector2 pos;

    //[SerializeField] private InputActionReference leftStick; //won't work in factory as object is created at runtime
    public Vector2 Position => Vector2.up;// leftStick.action.ReadValue<Vector2>();
    public event Action<Vector2> PosByEvent;
    public string Name => this.GetType().Name;
    
    public event Action<InputControllerType> DeviceConnected;
    private void Awake() => Setup();
    public void Setup()
    {
        HubFactory.AddInputDevice(this);
        //leftStick?.asset.Enable();
        DeviceConnected?.Invoke(InputControllerType.XBox);

        Debug.Log($"{this.GetType().Name} added to input devices");
    }


    private void OnDestroy()
    {
        //leftStick?.asset.Disable();
        HubFactory.RemoveInputDevice(this);
        Debug.Log($"{this.GetType().Name} removed from input devices");
    }

    void Update()
    {
        //pos = leftStick.action.ReadValue<Vector2>();
        if (pos == Vector2.zero) return;
        PosByEvent?.Invoke(pos);
        //print(Position);
    }
}
