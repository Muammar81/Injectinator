using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class FactoryTest : MonoBehaviour
{
    [SerializeField] private RectTransform rectTrans;
    [SerializeField] private RectTransform buttonPrefab;


    private void xOnEnable()
    {
        IEnumerable<string> inputsNames = HubFactory.GetInputNames();

        foreach (string inputName in inputsNames)
        {
            RectTransform btn = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, rectTrans);
            btn.name = btn.GetComponentInChildren<Text>().text = inputName;

            IInputDevice inputDevice = HubFactory.GetInput<IInputDevice>(inputName);
            inputDevice.DeviceConnected += InputDevice_deviceConnected;
        }
    }

    private void InputDevice_deviceConnected(InputControllerType obj) =>
        print(Enum.GetName(typeof(InputControllerType), obj) + " connected");
}
