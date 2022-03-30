using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHandler : Singleton<LineHandler>
{
    [SerializeField] LineVisualizer visualizerPrefab;
    [SerializeField] OldRails RailsPrefab;

    List<Line> lines = new List<Line>();

    public Line PickUpLine(ITrackpointCreator tpc)
    {
        foreach (Line line in lines)
        {
            if (line.TPCIsLast(tpc))
                return line;
        }

        return null;
    }


    public bool CityIsPartOfAnyLine(ITrackpointCreator tpc) {
        foreach (var line in lines)
        {
            if (line.Contains(tpc))
                return true;
        }

        return false;
    }

    public OldRails CreateRailsFor(Line line)
    {
        OldRails rails = Instantiate(RailsPrefab);
        rails.Init(line);
        return rails;
    }

    public bool DoesLineSegmentIntersectsWithAnyLine(LineSegment newSegment) {
        foreach (Line line in lines)
        {
            foreach (LineSegment segment in line.GetLineSegments())
            {
                if (newSegment.DoesIntersectWith(segment) && newSegment.first != segment.second
                    && (newSegment.second != segment.first)
                    && (newSegment.first != segment.first)
                    && (newSegment.second != segment.second))
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal Line CreateNewLine(ITrackpointCreator trackpointC)
    {
        Line line = new Line(trackpointC);
        lines.Add(line);
        Instantiate(visualizerPrefab).Display(line);
        return line;
    }
}