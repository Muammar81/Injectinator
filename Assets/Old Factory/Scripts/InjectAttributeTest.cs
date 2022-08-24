using UnityEngine;

public class InjectAttributeTest : MonoBehaviour,IInjectable
{
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


}
