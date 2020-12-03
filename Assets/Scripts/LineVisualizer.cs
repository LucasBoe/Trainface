using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineVisualizer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    Line line;

    public void Display(Line line) {
        this.line = line;
        lineRenderer.startColor = line.Color;
        lineRenderer.endColor = line.Color;
    }

    private void Update()
    {
        if (line != null)
        {
            City[] cities = line.GetCities();

            lineRenderer.positionCount = cities.Length;
            for (int i = 0; i < cities.Length; i++)
                lineRenderer.SetPosition(i, cities[i].transform.position);

        } else
            Destroy(gameObject);
    }
}
