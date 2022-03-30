using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RailTileData : CustomPreviewTileData
{
    public TileDataConnection[] Connections;

    public override void DrawConnections(Vector3 position, int offset)
    {
        foreach (TileDataConnection connection in Connections)
        {
            Vector3 p1 = position + (connection.From.Offset(offset).ToDirectionVector() / 2f);
            Vector3 p2 = position + (connection.To.Offset(offset).ToDirectionVector() / 2f);

            Debug.DrawLine(p1, p2, Color.yellow, duration: 0.5f);
        }
    }
}
