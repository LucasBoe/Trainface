using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileDirection
{
    North,
    East,
    South,
    West,
}

public static class TileDirectionUtil
{
    public static Quaternion ToTileRotation(this int offset)
    {
        return Quaternion.Euler(0, offset * 90, 0);
    }

    public static float ToAngle(this TileDirection tileDirection)
    {
        return (int)tileDirection * 90;
    }

    public static TileDirection Offset(this TileDirection tileDirection, int offset)
    {
        int raw = (int)tileDirection + offset;
        return (TileDirection)(raw % 4);
    }

    public static TileDirection Mirror(this TileDirection tileDirection)
    {
        return Offset(tileDirection, 2);
    }

    public static Vector3 ToDirectionVector(this TileDirection tileDirection)
    {
        switch (tileDirection)
        {
            case TileDirection.North:
                return Vector3.forward;

            case TileDirection.East:
                return Vector3.right;

            case TileDirection.South:
                return Vector3.back;
        }

        return Vector3.left;
    }
}
