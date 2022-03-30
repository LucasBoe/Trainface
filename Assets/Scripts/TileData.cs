using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileData : ScriptableObject
{
    [SerializeField] public GameObject visualizationPrefab;
    public abstract bool HasCustomPreview { get; }

    public virtual GameObject GetPreview()
    {
        return visualizationPrefab;
    }

    public virtual void DrawConnections(Vector3 position, int offset) { }
}
