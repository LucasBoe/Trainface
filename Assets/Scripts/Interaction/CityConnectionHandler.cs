using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityConnectionHandler : Singleton<CityConnectionHandler>
{
    [SerializeField] LineRenderer lineRenderer;
    Line line;

    internal void TryStartConnectionAt(City city)
    {
        Debug.Log("try start at city" + city.name);

        //pick up existing line
        line = Game.LineHandler.PickUpLine(city);

        //create new if city is producer
        if (line == null && city.Produces())
            line = Game.LineHandler.CreateNewLine(city);
    }

    internal void TryEndConnectionAt(City city)
    {
        if (line == null)
            return;

        LineSegment lineSegment = new LineSegment(line.GetCities()[line.GetCities().Length - 1].transform.position.To2D(), Game.PlayerInteraction.hitPosition.To2D());
        if (!Game.LineHandler.DoesLineSegmentIntersectsWithAnyLine(lineSegment))
            line.AddCity(city);

        line = null;
    }

    private void Update()
    {
        if (line != null)
        {
            City[] cities = line.GetCities();
            LineSegment segment = new LineSegment(cities[cities.Length - 1].transform.position.To2D(), Game.PlayerInteraction.hitPosition.To2D());
            Color c = Game.LineHandler.DoesLineSegmentIntersectsWithAnyLine(segment) ? Color.red : Color.green;

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
