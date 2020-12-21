using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityConnectionHandler : Singleton<CityConnectionHandler>
{
    [SerializeField] LineRenderer lineRenderer;
    Line line;

    internal void TryStartConnectionAt(ITrackpointCreator trackpointC)
    {
        Debug.Log("try start at city" + trackpointC.GetName());

        //pick up existing line
        line = Game.LineHandler.PickUpLine(trackpointC);

        //create new if city is producer
        if (line == null && trackpointC.Produces())
            line = Game.LineHandler.CreateNewLine(trackpointC);
    }

    internal void TryEndConnectionAt(ITrackpointCreator city)
    {
        if (line == null)
            return;

        Trackpoint[] trackpoints = line.GetTrackpoints();
        LineSegment lineSegment = new LineSegment(trackpoints[trackpoints.Length - 1].GetLocation2D(), Game.PlayerInteraction.hitPosition.To2D());
        if (!Game.LineHandler.DoesLineSegmentIntersectsWithAnyLine(lineSegment) && !Game.LineHandler.CityIsPartOfAnyLine(city))
            line.Add(city);

        line = null;
    }

    private void Update()
    {
        if (line != null)
        {
            Trackpoint[] trackpoints = line.GetTrackpoints();
            LineSegment segment = new LineSegment(trackpoints[trackpoints.Length - 1].GetLocation2D(), Game.PlayerInteraction.hitPosition.To2D());
            Color c = Game.LineHandler.DoesLineSegmentIntersectsWithAnyLine(segment) ? Color.red : line.Color;

            lineRenderer.startColor = c;
            lineRenderer.endColor = c;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, segment.first.To3D());
            lineRenderer.SetPosition(1, segment.second.To3D());

        } else {
            lineRenderer.positionCount = 0;
            lineRenderer.SetPositions(new Vector3[0]);
        }
    }

    internal void AbortPlacement()
    {
        line = null;
    }
}
