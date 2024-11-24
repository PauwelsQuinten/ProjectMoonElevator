using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CameraReference
{
    public bool useConstant;
    public Camera constantValue;
    public CameraVariable variable;

    public Camera value
    {
        get
        {
            return useConstant ? constantValue :
                                 variable.value;
        }
    }
}
