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

        Material mat = new Material(dotRenderer.material);
        mat.color = line.Color;

        dotRenderer.material = mat;
        lineRenderer.material = mat;

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

            dotPosition = (Mathf.Sin(Time.time * dotSpeed / line.Length) + 1) / 2; ;

            //dot
            //UpdateDot();

        }
        else
            Destroy(gameObject);
    }

    private void UpdateDot()
    {
        Trackpoint before = null;
        float d = 0;
        float posTarget = dotPosition * length;

        foreach (Trackpoint current in line.GetTrackpoints()) {
            if (before != null)
            {
                float distanceToAdd = Vector3.Distance(before.GetLocation(), current.GetLocation());

                if (d < posTarget && (d + distanceToAdd) > posTarget) {
                    float pos = (posTarget - d) / distanceToAdd;
                    dotRenderer.transform.position = before.GetLocation() * (1-pos) + current.GetLocation() * pos;

                    //if (pos < 0.01)
                    //    line.Train.CheckPassTEMP(before);
                    //else if (pos > 0.99f)
                    //    line.Train.CheckPassTEMP(current);

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
        Trackpoint[] trackpoints = line.GetTrackpoints();

        lineRenderer.positionCount = trackpoints.Length;
        for (int i = 0; i < trackpoints.Length; i++)
            lineRenderer.SetPosition(i, trackpoints[i].GetLocation() + Vector3.up * 0.1f);
    }

    private void OnDestroy()
    {
        line.OnChangeLength -= OnUpdateLength;
    }
}
