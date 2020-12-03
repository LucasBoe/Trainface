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

    private Color color;
    public Color Color {
        get {
            return color;
        }
    }

    public Line (City startingCity) {
        cities.Add(startingCity);
        color = startingCity.Color;
    }

    public void AddCity (City city) {
        cities.Add(city);
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
}
