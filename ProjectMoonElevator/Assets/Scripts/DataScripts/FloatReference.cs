using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FloatReference
{
    public bool useConstant;
    public float constantValue;
    public FloatVariable variable;

    public float value
    {
        get
        {
            return useConstant ? constantValue :
                                 variable.value;
        }
    }
}
