using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RailSection
{
    [SerializeField] Tile from, to;
    [SerializeField] TileDirection In, Out;
    [SerializeField] Vector3 Start, End;
    [SerializeField] bool IsCurve;
    Vector3 center;

    public RailSection(Tile owner, Tile _from, Tile _to, TileDirection _in, TileDirection _out)
    {
        from = _from;
        to = _to;

        In = _in;
        Out = _out;

        IsCurve = In.Mirror() != Out;

        center = owner.transform.position;
        Start = center + _in.ToDirectionVector() * 0.5f;
        End = center + _out.ToDirectionVector() * 0.5f;
    }

    internal bool IsReverse(RailSection section)
    {
        return (Vector3.Distance(Start, section.End) > 0.5f && Vector3.Distance(End, section.Start) > 0.5f);
    }

    internal RailSection GetNext(TrainDirection direction)
    {
        return direction == TrainDirection.Forward ? to?.GetSection() : from?.GetSection();
    }

    internal Quaternion GetRotation(TrainDirection direction, float progression)
    {
        float _in = In.Mirror().ToAngle();
        float _out = Out.ToAngle();

        progression = direction.Invert(progression);

        return Quaternion.Euler(0, Mathf.LerpAngle(_in, _out, progression) + (direction == TrainDirection.Forward ? 0 : 180), 0);
    }

    public Vector3 GetPosition(TrainDirection forward, float progression)
    {
        progression = forward.Invert(progression);

        if (!IsCurve)
            return Vector3.Lerp(Start, End, progression);

        Vector3 ab = Vector3.Lerp(Start, center, progression);
        Vector3 bc = Vector3.Lerp(center, End, progression);
        return Vector3.Lerp(ab, bc, progression);
    }
}
