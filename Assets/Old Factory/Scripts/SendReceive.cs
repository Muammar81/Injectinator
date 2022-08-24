using Born.Framework.Events;
using UnityEngine;

public class SendReceive : MonoBehaviour, IXEventListener<float>
{
    private readonly XEvent<float> xFloatEvent = new XEvent<float>();
    private void OnEnable() => xFloatEvent.RegisterListener(this);
    private void OnDisable() => xFloatEvent.UnregisterListener(this);

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float rand = Random.Range(0, 99);
            print($"publishing {rand}");
            xFloatEvent?.Raise(rand);
        }
    }
    public void OnEventRaised<T>(T item) => print(item);
}
