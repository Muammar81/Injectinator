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
    [Inject]
    public void Construct(params object[] args)
    {
        print(args.Length);
    }

    [Inject]
    private int AnotherConstruct(float x)
    {
        return 0;
    }

    [Inject]
    private void Construct3(float x, string s)
    {
    }

    [Inject]
    public void GetKeyValuePairs(int i, float f)
    {
    }
    
    #endregion


}
