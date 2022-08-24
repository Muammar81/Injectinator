using System;
using System.Collections.Generic;

    public class Builder
    {
        private List<ServiceData> _services = new List<ServiceData>();
        
        public void RegisterSingleton<TService>()=>
            _services.Add(new ServiceData(typeof(TService), ServiceLifetime.Singleton));
        
        public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService=>
            _services.Add(new ServiceData(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
        
        /// <summary>
        /// Registers dependency services to the services collection
        /// </summary>
        /// <param name="lifetime">Lifetime of the service (Will default to transient when not provided)</param>
        /// <typeparam name="TService">Service type, usually a high level interface</typeparam>
        public void Register<TService>(ServiceLifetime lifetime = ServiceLifetime.Transient) =>
            _services.Add(new ServiceData(typeof(TService), lifetime));
        
        public void Register<TService, TImplementation>(ServiceLifetime lifetime = ServiceLifetime.Transient) where TImplementation : TService =>
            _services.Add(new ServiceData(typeof(TService), typeof(TImplementation), lifetime));

        
        public Container GenerateContainer()=> new Container(_services);
    }


public class ServiceData
{
    public Type ServiceType { get; }
        
    public Type ImplementationType { get; }

    public object Implementation { get; internal set; }

    public ServiceLifetime Lifetime { get; }

    public ServiceData(object implementation, ServiceLifetime lifetime)
    {
        ServiceType = implementation.GetType();
        Implementation = implementation;
        Lifetime = lifetime;
    }
        
    public ServiceData(Type serviceType, ServiceLifetime lifetime)
    {
        ServiceType = serviceType;
        Lifetime = lifetime;
    }
        
    public ServiceData(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        ServiceType = serviceType;
        Lifetime = lifetime;
        ImplementationType = implementationType;
    }
}

public enum ServiceLifetime
{
    Singleton,
    Transient
}