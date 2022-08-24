using System;
using System.Collections.Generic;
using System.Linq;

public class Container 
{
    private readonly List<ServiceData> _serviceDescriptors;

    public Container(List<ServiceData> serviceDescriptors)=> _serviceDescriptors = serviceDescriptors;

    public object GetService(Type serviceType)
    {
        var desciptor = _serviceDescriptors
            .SingleOrDefault(x => x.ServiceType == serviceType);

        if (desciptor == null)
            throw new Exception($"Unregisteed service: {serviceType.Name}");

        if (desciptor.Implementation != null)
            return desciptor.Implementation;

        var actualType = desciptor.ImplementationType ?? desciptor.ServiceType;

        if (actualType.IsAbstract || actualType.IsInterface)
            throw new Exception("Cannot instantiate abstract classes or interfaces");

        var constructorInfo = actualType.GetConstructors().First();

        var parameters = constructorInfo.GetParameters()
            .Select(x => GetService(x.ParameterType)).ToArray();

        var implementation = Activator.CreateInstance(actualType, parameters);

        if (desciptor.Lifetime == ServiceLifetime.Singleton)
            desciptor.Implementation = implementation;

        return implementation;
    }

    public T GetService<T>()
    {
        return (T)GetService(typeof(T));
    }
}
