using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoolReference
{
    public bool useConstant;
    public bool constantValue;
    public BoolVariable variable;

    public bool value
    {
        get
        {
            return useConstant ? constantValue :
                                 variable.value;
        }
    }
}
