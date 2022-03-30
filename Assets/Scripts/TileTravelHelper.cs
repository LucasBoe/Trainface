using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileTravelHelper
{
    internal static TravelResult Travel(RailSection section, TrainDirection direction, float tilePosition)
    {
        TravelResult result = new TravelResult();

        if (tilePosition > 1f)
        {
            result.UpdateTilePosition = true;

            tilePosition -= 1f;
            result.NewSection = section.GetNext(direction);

            if (result.NewSection == null || result.NewSection.IsReverse(section))
                result.ChangeDirection = true;
        }

        result.Position = section.GetPosition(direction, tilePosition);
        result.Rotation = section.GetRotation(direction, tilePosition);

        return result;
    }
}

public class TravelResult
{
    public Vector3 Position;
    public Quaternion Rotation;

    public bool ChangeDirection = false;
    public RailSection NewSection = null;

    public bool UpdateTilePosition = false;
    public float NewTilePosition;
}