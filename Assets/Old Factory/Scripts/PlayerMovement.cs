using Born.Framework.Core;
using Core;
using UnityEngine;

namespace Born.Gameplay
{
    public class PlayerMovement : MonoBehaviour
    {
        private XBoxController controllerDevice;
        private float speed = 5;
        private void Awake()
        {
            controllerDevice = HubFactory.GetInputDevice<XBoxController>();
            //HubFactory.GetInputDevice<IInputDevice>().posByEvent += PlayerMovement_posByEvent;

            if (controllerDevice == null)
                Debug.LogError("No controllers found!",this);
        }

        private void Update()
        {
            controllerDevice ??=  HubFactory.GetInputDevice<XBoxController>();
            Move(controllerDevice.Position);
        }

        private void PlayerMovement_posByEvent(Vector2 pos) => Move(pos);
        private void Move(Vector2 newPos) => transform.Translate(newPos * speed * Time.deltaTime, Space.World);
    }
}






