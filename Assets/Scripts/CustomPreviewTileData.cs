using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPreviewTileData : TileData
{
    public override bool HasCustomPreview => true;
    public GameObject CustomPreview;

    public override GameObject GetPreview()
    {
        return CustomPreview;
    }
}
