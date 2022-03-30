using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util 
{
    public static void DestroyAllChildren(this Transform transform)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
    }
}
