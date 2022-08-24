using UnityEngine;
public class InjectAttributeTest : MonoBehaviour, IService// ,IInjectable 
{
    #region Injectable Fields
    [Inject] private IService serviceOne;
    [Inject] private IService serviceTwo;
    public void Log()
    {
        serviceOne.Log();
    }
    #endregion
    
    #region Injectable Methods
    [Inject] public void FakeConstruct(params object[] args)=> print(args.Length);
    [Inject] private int AnotherMethod(float x,float y) =>  0;
    #endregion


}
