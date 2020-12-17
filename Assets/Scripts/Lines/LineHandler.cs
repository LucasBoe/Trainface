using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHandler : Singleton<LineHandler>
{
    [SerializeField] LineVisualizer visualizerPrefab;
    [SerializeField] Rails RailsPrefab;

    List<Line> lines = new List<Line>();

    public Line PickUpLine(City city) {
        foreach (Line line in lines)
        {
            if (line.CityIsLast(city))
                return line;
        }

        return null;
    }

    public bool CityIsPartOfLine(City city) {
        foreach (var line in lines)
        {
            if (line.Contains(city))
                return true;
        }

        return false;
    }

    public Rails CreateRailsFor(Line line)
    {
        Rails rails = Instantiate(RailsPrefab);
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

    internal Line CreateNewLine(City city)
    {
        Line line = new Line(city);
        lines.Add(line);
        Instantiate(visualizerPrefab).Display(line);
        return line;
    }
}