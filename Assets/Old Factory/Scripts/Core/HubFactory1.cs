using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Born.Framework.Core
{
    public static class InjectionFactory
    {
        private static Dictionary<string, Type> serviceNames = new Dictionary<string, Type>();
        private static Dictionary<Type, object> serviceObjects = new Dictionary<Type, object>();
        private static bool IsInitialized => serviceNames != null;

        //Adding types to dictionary
        private static void Initialize()
        {
            //if (IsInitialized) return;

            var inputTypes = GetTypesInDefaultAssembly<global::Core.IInjectable>();

            foreach (Type type in inputTypes)
            {
                if(serviceNames.ContainsKey(type.Name)) continue;
                serviceNames.Add(type.ToString(), type);
                Debug.Log($"Factory: {type.Name} found and added to dictionary");
            }
        }

        //terrible & desperate idea
        private static IInputDevice FindInputsInScene<T>(T ty) where T : IInputDevice
        {
            var o = UnityEngine.Object.FindObjectsOfType(ty.GetType()).FirstOrDefault();
            return o as IInputDevice;
        }

        public static void AddInputDevice(IInputDevice inputDevice)
        {
            var devType = inputDevice.GetType();

            if (!serviceObjects.ContainsKey(devType))
            {
                serviceObjects.Add(devType, inputDevice);
            }
        }

        public static void RemoveInputDevice(IInputDevice inputDevice)
        {
            var devType = inputDevice.GetType();

            if (!serviceObjects.ContainsKey(devType))
            {
                serviceObjects.Remove(devType);
            }
        }

        /// <summary>
        /// Returns a list of available input types
        /// </summary>
        /// <returns>Input key names</returns>
        public static IEnumerable<string> GetServiceNames()
        {
            Initialize();
            return serviceNames.Keys;
        }


        public static T GetType<T>(params object[] args) where T : global::Core.IInjectable, new()
        {
            Initialize();
            var key = nameof(T);
            if (!serviceNames.ContainsKey(key)) return new T();

            Type type = serviceNames[key];
            global::Core.IInjectable service;

            //use fake constructor for MonoBehaviours
            if(type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                service = Activator.CreateInstance(type) as global::Core.IInjectable;
                service.Construct();
            }
            //otherwise use actual constructor
            else
            {
                service = Activator.CreateInstance(type,args) as global::Core.IInjectable;
            }

            return (T)service;
        }

        //Helpers
        private static Assembly GetDefualtAssembly() => AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "Assembly-CSharp");
        private static IEnumerable<Type> GetTypesInDefaultAssembly<T>() => GetDefualtAssembly().GetTypes().Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
    }


}

public interface IInjectable
{
    [Inject] void Construct(params object[] args);
}
