using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
public class Inject : Attribute
{
    public  Inject()
    {
        
    }
}
