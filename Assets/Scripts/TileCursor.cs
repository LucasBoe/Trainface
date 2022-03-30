using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCursor : MonoBehaviour
{
    [SerializeField] Transform previewParent;
    private void OnEnable()
    {
        InHandInteractionMode.ChangedPreviewTile += OnChangedTileMode;
    }

    private void OnDisable()
    {
        InHandInteractionMode.ChangedPreviewTile -= OnChangedTileMode;
    }

    private void OnChangedTileMode(TileData tile, int offset)
    {
        SetPreviewTile(tile, offset);
    }

    private void SetPreviewTile(TileData tile, int offset)
    {
        previewParent.DestroyAllChildren();

        if (tile != null)
        {
            GameObject preview = tile.GetPreview();

            if (preview != null)
                Instantiate(tile.GetPreview(), previewParent.position, offset.ToTileRotation(), previewParent);

            tile.DrawConnections(previewParent.position, offset);
        }
    }

    public void UpdateCursorPosition(Vector3 onGrid)
    {
        transform.position = Vector3.Lerp(transform.position, onGrid, Time.deltaTime * 10f);
    }
}