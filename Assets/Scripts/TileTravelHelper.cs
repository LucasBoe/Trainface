using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileTravelHelper
{
    internal static PositionRotationPair Travel(RailSection section, TrainDirection direction, float tilePosition)
    {
        PositionRotationPair result = new PositionRotationPair();
        result.Position = section.GetPosition(direction, tilePosition);
        result.Rotation = section.GetRotation(direction, tilePosition);
        return result;
    }

    public static SectionSwitch GetNextSection(RailSection oldSection, TrainDirection direction)
    {
        SectionSwitch result = new SectionSwitch();

        result.PreviousSection = oldSection;

        RailSection next = oldSection.GetNext(direction);

        if (next == null || next.IsReverse(oldSection))
            result.Direction = direction.Inverse();
        else
            result.Direction = direction;

        result.NewSection = next != null ? next : oldSection;

        return result;
    }
}

public class PositionRotationPair
{
    public Vector3 Position;
    public Quaternion Rotation;
}

[System.Serializable]
public class SectionSwitch
{
    public RailSection PreviousSection = null;
    public TrainDirection Direction;
    public RailSection NewSection = null;
    public SectionSwitch NextSwitch;
}