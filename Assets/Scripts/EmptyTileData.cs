using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EmptyTileData : TileData
{
    public override bool HasCustomPreview => false;

    public override GameObject GetPreview()
    {
        return null;
    }
}
