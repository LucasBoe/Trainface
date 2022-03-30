using System;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    List<Trackpoint> trackpoints = new List<Trackpoint>();
    float length;
    public float Length {
        get {
            return length;
        }
    }

    private Color color;
    public Color Color {
        get {
            return color;
        }
    }

    private OldRails rails;
    public OldRails Rails
    {
        get
        {
            if (rails == null)
                rails = Game.LineHandler.CreateRailsFor(this);

            return rails;
        }
    }

    private OldTrain train;
    public OldTrain Train {
        get {
            if (train == null)
                train = Game.TrainHandler.SpawnTrain(this);

            return train;
        }
    }

    public Goods Goods { get; internal set; }

    public System.Action<float> OnChangeLength;

    //constructor
    public Line (ITrackpointCreator startingTPC) {
        Debug.Log("create new line from trackpoint creator");
        trackpoints.AddRange(startingTPC.CreateTrackpoints());
        Goods = startingTPC.GetGoods();
        color = startingTPC.GetColor();
    }

    public void Add (ITrackpointCreator newTPC) {
        if (Rails != null)
            Debug.Log("Rails found");

        Debug.Log("add new trackpoints from trackpoint creator");

        trackpoints.AddRange(newTPC.CreateTrackpoints());
        UpdateConnections();
        UpdateLength();
        Train.Rail(this);
        Game.LevelHandler.ResetProgression();
    }

    private void UpdateConnections()
    {
        foreach (Trackpoint tp in trackpoints)
            tp.Line = this;
        
    }

    private void UpdateLength()
    {
        length = 0;

        Trackpoint before = null;

        foreach (var current in trackpoints)
        {
            if (before != null)
                length += Vector3.Distance(before.GetLocation(), current.GetLocation());

            before = current;
        }
        OnChangeLength?.Invoke(length);
    }

    public bool TPCIsLast(ITrackpointCreator tpc)
    {
        //city is first or last of a line
        if (trackpoints.Count > 0)
        {
            if (trackpoints[trackpoints.Count - 1].GetCreator() == tpc)
                return true;
        }

        return false;
    }

    public bool Contains (ITrackpointCreator tpc) {

        foreach (Trackpoint tp in trackpoints)
        {
            if (tp.GetCreator() == tpc)
                return true;
        }

        return false;
    }

    public Trackpoint[] GetTrackpoints()
    {
        return trackpoints.ToArray();
    }

    public LineSegment [] GetLineSegments ()
    {
        List<LineSegment> segments = new List<LineSegment>();

        Trackpoint before = null;
        foreach (Trackpoint current in trackpoints)
        {
            if (before != null)
                segments.Add(new LineSegment(before.GetLocation2D(), current.GetLocation2D()));

            before = current;
        }

        return segments.ToArray();
    }

    public Trackpoint GetNextTrackpoint(Trackpoint trackpoint, bool directionIsForward)
    {
        for (int i = 0; i < trackpoints.Count; i++)
        {
            if (trackpoint == trackpoints[i])
            {
                if (directionIsForward && (i + 1) < trackpoints.Count)
                {
                    return trackpoints[i + 1];
                }
                if (!directionIsForward && i > 0)
                {
                    return trackpoints[i - 1];
                }
                else
                {
                    return null;
                }
            }
        }

        return null;
    }
}