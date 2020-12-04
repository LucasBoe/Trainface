using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineVisualizer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] MeshRenderer dotRenderer;
    [SerializeField] float dotSpeed = 10;
    Line line;
    float length;
    float dotPosition = 0.5f;

    public void Display(Line line) {
        this.line = line;

        lineRenderer.startColor = line.Color;
        lineRenderer.endColor = line.Color;
        dotRenderer.material = new Material(dotRenderer.material);
        dotRenderer.material.color = line.Color;

        line.OnChangeLength += OnUpdateLength;
    }

    private void OnUpdateLength(float _length) {
        length = _length;
    }

    private void Update()
    {
        if (line != null)
        {
            //line
            UpdateLinePositions();

            dotPosition = (Mathf.Sin(Time.time * dotSpeed / length) + 1) / 2;

            //dot
            UpdateDot();

        }
        else
            Destroy(gameObject);
    }

    private void UpdateDot()
    {
        City before = null;
        float d = 0;
        float posTarget = dotPosition * length;

        foreach (City current in line.GetCities()) {
            if (before != null)
            {
                float distanceToAdd = Vector3.Distance(before.transform.position, current.transform.position);

                if (d < posTarget && (d + distanceToAdd) > posTarget) {
                    float pos = (posTarget - d) / distanceToAdd;
                    dotRenderer.transform.position = before.transform.position * (1-pos) + current.transform.position * pos;
                    return;
                } else {
                    d += distanceToAdd;
                }
            }
            before = current;
        }
    }

    private void UpdateLinePositions()
    {
        City[] cities = line.GetCities();

        lineRenderer.positionCount = cities.Length;
        for (int i = 0; i < cities.Length; i++)
            lineRenderer.SetPosition(i, cities[i].transform.position);
    }

    private void OnDestroy()
    {
        line.OnChangeLength -= OnUpdateLength;
    }
}
