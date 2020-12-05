using System.Collections.Generic;
using UnityEngine;

public class Line
{
    List<City> cities = new List<City>();
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

    private Train train;
    public Train Train {
        get {
            if (train == null)
                train = Game.TrainHandler.SpawnTrain(this);

            return train;
        }
    }

    public System.Action<float> OnChangeLength;

    //constructor
    public Line (City startingCity) {
        cities.Add(startingCity);
        color = startingCity.Goods.Data.color;
    }

    public void AddCity (City city) {
        cities.Add(city);
        UpdateLength();
        Game.LevelHandler.ResetProgression();
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