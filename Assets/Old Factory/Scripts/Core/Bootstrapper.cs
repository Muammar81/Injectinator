using Core;
using UnityEngine;

namespace Born.Framework.Core
{
    public class Bootstrapper
    {
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            Object obj = Object.Instantiate(Resources.Load("Systems"));
            Object.DontDestroyOnLoad(obj);

            Debug.Log("Bootstrapper started. Loading Systems...");
            obj.name = obj.name.Replace("(Clone)", " - Bootstrapper");

            HubFactory.GetInputNames();

            //Addressables variant
            //Object.DontDestroyOnLoad(Addressables.InstantiateAsync(("Systems").WaitForCompletion));
        }
    }
}
