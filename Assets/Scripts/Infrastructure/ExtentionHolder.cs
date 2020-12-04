using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtentionHolder
{
    public static Vector2 To2D (this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    public static Vector3 To3D(this Vector2 vector2)
    {
        return new Vector3(vector2.x,0, vector2.y);
    }
}
