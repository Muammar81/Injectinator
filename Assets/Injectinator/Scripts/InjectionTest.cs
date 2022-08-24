using System;
using UnityEngine;

public class InjectionTest : MonoBehaviour
{
    private void Start()
    {
        var builder = new Builder();

        #region Register Services
        //Register By type
        builder.Register<RandomGuidProvider>();
        
        builder.Register<IService, ServiceOne>();
        builder.Register<IRandomGuidProvider, RandomGuidProvider>();
        #endregion Register services

        var container = builder.GenerateContainer();
        var serviceOne = container.GetService<IService>();

        serviceOne.Log();
    }
}

public interface IService
{
    void Log();
}

/// <summary>
/// Non-Monobehaviour Service example Injectable through constructor
/// </summary>
public class ServiceOne : IService
{
    private readonly IRandomGuidProvider _randomGuidProvider;
    public ServiceOne(IRandomGuidProvider randomGuidProvider)
    {
        _randomGuidProvider = randomGuidProvider;
    }

    public void Log()
    {
        Debug.Log(_randomGuidProvider.RandomGuid);
    }
}

public interface IRandomGuidProvider
{
    Guid RandomGuid { get; }
}

public class RandomGuidProvider : IRandomGuidProvider
{
    public Guid RandomGuid { get; } = Guid.NewGuid();
}