using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Core
{
    public static class HubFactory
    {
        private static Dictionary<string, Type> inputsByName = new Dictionary<string, Type>();
        private static Dictionary<Type, object> inputDevices = new Dictionary<Type, object>();
        private static bool IsInitialized => inputsByName != null;

        //Adding types to dictionary
        private static void Initialize()
        {
            //if (IsInitialized) return;

            var inputTypes = GetTypesInDefaultAssembly<IInputDevice>();

            foreach (Type type in inputTypes)
            {
                if(inputsByName.ContainsKey(type.Name)) continue;
                inputsByName.Add(type.ToString(), type);
                Debug.Log($"Factory: {type.Name} Input Device found and added to dictionary");

                //var device = Activator.CreateInstance(type) as IInputDevice;
                //var device = Activator.CreateInstance(type) as IInputDevice;
                //device?.Setup();
                //inputDevices.Add(type, device);
            }
        }


        public static void AddInputDevice(IInputDevice inputDevice)
        {
            var devType = inputDevice.GetType();

            if (!inputDevices.ContainsKey(devType))
            {
                inputDevices.Add(devType, inputDevice);
            }
        }

        public static void RemoveInputDevice(IInputDevice inputDevice)
        {
            var devType = inputDevice.GetType();

            if (!inputDevices.ContainsKey(devType))
            {
                inputDevices.Remove(devType);
            }
        }

        /// <summary>
        /// Returns a list of available input types
        /// </summary>
        /// <returns>Input key names</returns>
        public static IEnumerable<string> GetInputNames()
        {
            Initialize();
            return inputsByName.Keys;
        }

        /// <summary>
        /// Retrieves an input type by key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputName">Input key name</param>
        /// <returns>Input type</returns>
        public static T GetInput<T>(string inputName)// where T : class, new()
        {
            Initialize();
            if (!inputsByName.ContainsKey(inputName)) return default;

            Type type = inputsByName[inputName];

            //if (typeof(T) == typeof(MonoBehaviour)) return default;
            
            return (T)Activator.CreateInstance(type);
        }

        public static T GetInputDevice<T>() where T : IInputDevice
        {
            Initialize();
            return (T)inputDevices[typeof(T)];
        }

        public static T GetInjectable<T>(params object[] args) where T : IInjectable, new()
        {
            Initialize();
            var key = nameof(T);
            if (!inputsByName.ContainsKey(key)) return new T();

            Type type = inputsByName[key];
            var obj = (T)Activator.CreateInstance(type);
            obj.Construct(args);
            return obj;
        }

    //Helpers
    private static Assembly GetDefualtAssembly() => AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "Assembly-CSharp");
        private static IEnumerable<Type> GetTypesInDefaultAssembly<T>() => GetDefualtAssembly().GetTypes().Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
    }

    public interface IInputDevice
    {
        string Name { get; }
        Vector2 Position { get; }
        event Action<InputControllerType> DeviceConnected;
        void Setup();

        event Action<Vector2> PosByEvent;
    }
    public enum InputControllerType { XBox, PS4 }

    public interface IInjectable
    {
        void Construct(params object[] args);
    }
}