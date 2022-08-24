using UnityEngine;
public class InjectAttributeTest : MonoBehaviour, IService// ,IInjectable 
{
    #region Injectable Fields
    [Inject] private IService service;
    public void Log()
    {
        print(service);
    }
    #endregion
    
    #region Injectable Methods
    [Inject] public void FakeConstruct(params object[] args)=> print(args.Length);
    [Inject] private int AnotherMethod(float x) =>  0;
    #endregion


}
