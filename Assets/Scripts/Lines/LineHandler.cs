using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHandler : Singleton<LineHandler>
{
    [SerializeField] LineVisualizer visualizerPrefab;

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

public class Line
{
    List<City> cities = new List<City>();
    float length;
    public System.Action<float> OnChangeLength;

    private Color color;
    public Color Color {
        get {
            return color;
        }
    }

    public Line (City startingCity) {
        cities.Add(startingCity);
        color = startingCity.Goods.Data.color;
    }

    public void AddCity (City city) {
        cities.Add(city);
        UpdateLength();
    }

    private void UpdateLength()
    {
        length = 0;

        City before = null;

        foreach (var current in cities)
        {
            if (before != null)
                length += Vector3.Distance(before.transform.position, current.transform.position);

            before = current;
        }
        OnChangeLength?.Invoke(length);
    }

    public bool CityIsLast(City city)
    {
        //city is first or last of a line
        if (cities.Count > 0 && cities[cities.Count - 1] == city)
            return true;

        return false;
    }

    public bool Contains (City city) {
        return cities.Contains(city);
    }

    public City[] GetCities () {
        return cities.ToArray();
    }

    public LineSegment [] GetLineSegments () {
        List<LineSegment> segments = new List<LineSegment>();

        City before = null;
        foreach (City current in cities)
        {
            if (before != null)
                segments.Add(new LineSegment(before.transform.position.To2D(), current.transform.position.To2D()));

            before = current;
        }

        return segments.ToArray();
    }
}