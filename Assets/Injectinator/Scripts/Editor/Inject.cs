using System;

namespace Injectinator.Runtime
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
    public class Inject : Attribute
    {
    }
}