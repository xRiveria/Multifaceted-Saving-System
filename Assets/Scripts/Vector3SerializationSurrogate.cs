﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// Serializing does not cover all of Unity's Classes, such as Vector3s and Quarternions.
/// That is why we have to create our own way of serializing these data, such as using Serialization Surrogates. 

public class Vector3SerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Vector3 vector3 = (Vector3)obj;
        info.AddValue("x", vector3.x);
        info.AddValue("y", vector3.y);
        info.AddValue("z", vector3.z);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Vector3 vector3 = (Vector3)obj;
        vector3.x = (float)info.GetValue("x", typeof(float));
        vector3.y = (float)info.GetValue("y", typeof(float));
        vector3.z = (float)info.GetValue("z", typeof(float));
        obj = vector3;
        return obj;
    }
}
