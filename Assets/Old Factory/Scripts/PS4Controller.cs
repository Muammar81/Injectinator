using Born.Framework.Core;
using System;
using Core;
using UnityEngine;

public class PS4Controller : MonoBehaviour, IInputDevice
{
    public string Name { get; }
    public Vector2 Position { get; }

    public event Action<InputControllerType> DeviceConnected;
    public event Action<Vector2> PosByEvent;

    public void Setup()
    {
        DeviceConnected?.Invoke(InputControllerType.PS4);
        Debug.Log($"{this.GetType().Name} added to input devices");
        PosByEvent?.Invoke(Vector2.down);
    }
}
