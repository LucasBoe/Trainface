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
        Debug.Log("start at city" + city.name);

        line = Game.LineHandler.PickUpLine(city);
        if (line == null) {
            line = Game.LineHandler.CreateNewLine(city);
        }
    }

    internal void TryEndConnectionAt(City city)
    {
        if (line != null && !Game.LineHandler.CityIsPartOfLine(city))
            line.AddCity(city);

        line = null;
    }

    private void Update()
    {
        if (line != null)
        {
            lineRenderer.startColor = line.Color;
            lineRenderer.endColor = line.Color;

            City[] cities = line.GetCities();

            lineRenderer.positionCount = cities.Length + 1;
            for (int i = 0; i <= cities.Length; i++)
            {
                if (i == cities.Length)
                    lineRenderer.SetPosition(i, Game.PlayerInteraction.hitPosition);
                else
                    lineRenderer.SetPosition(i, cities[i].transform.position);
            }

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
