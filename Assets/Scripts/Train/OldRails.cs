using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldRails : MonoBehaviour
{
    [SerializeField] Transform RailElementPrefab;
    [SerializeField] List<Trackpoint> trackpoints = new List<Trackpoint>();
    [SerializeField] Material coloredMaterial;

    Line line;

    public void Init(Line line)
    {
        this.line = line;
        line.OnChangeLength += UpdateTracks;

        if (Game.Settings.coloredTracks)
        {
            coloredMaterial = new Material(coloredMaterial);
            coloredMaterial.color = line.Color;
        }
    }

    private void OnDestroy()
    {
        line.OnChangeLength -= UpdateTracks;
    }

    private void UpdateTracks(float newLength)
    {
        for (int i = transform.childCount -1 ; i >= 0 ; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        trackpoints.Clear();

        Debug.Log("Update tracks...");

        Trackpoint before = null;
        foreach (Trackpoint current in line.GetTrackpoints())
        {
            Debug.Log("loop through cities");

            if (before != null)
            {
                Vector2 first = before.GetLocation().To2D();
                Vector2 second = current.GetLocation().To2D();

                float length = (Vector2.Distance(first, second) * 2);
                Quaternion rotation = Quaternion.LookRotation((second - first).To3D());
                for (int i = 0; i < length; i++)
                {
                    MeshRenderer meshRenderer = Instantiate(RailElementPrefab, Vector2.Lerp(first, second, (float)i / length).To3D(), rotation, transform).GetComponent<MeshRenderer>();
                    Material[] materials = meshRenderer.materials;
                    materials[1] = coloredMaterial;
                    meshRenderer.materials = materials;
                }
            }

            trackpoints.Add(current);

            before = current;
        }


        //foreach (LineSegment ls in line.GetLineSegments())
        //{
        //    float length = (ls.Length * 2);
        //    for (int i = 0; i < length; i++)
        //    {
        //        Instantiate(RailElementPrefab, Vector2.Lerp(ls.first, ls.second, (float)i / length).To3D(), Quaternion.LookRotation((ls.second - ls.first).To3D()), transform);
        //    }
        //}

    }

    public Trackpoint GetClosestTrackpoint(Vector3 position)
    {
        //if (trackpoints.Count == 0)
        //    UpdateTracks(0f);

        Debug.Log("Tried fidning closest trackpoint from " + trackpoints.Count +" trackpoints");

        Trackpoint closestPoint = null;
        float distance = float.MaxValue;

        foreach (Trackpoint tp in trackpoints)
        {
            float dist = Vector3.Distance(position,tp.GetLocation());
            if (dist < distance)
            {
                closestPoint = tp;
                distance = dist;
            }
        }

        return closestPoint;
    }
}

[System.Serializable]
public class Trackpoint
{
    [SerializeField] ITrackpointCreator origin;
    [SerializeField] Vector3 location;
    [SerializeField] Trackpoint before;
    [SerializeField] Trackpoint next;
    [SerializeField] float orientation;
    float distanceToNext;

    public Line Line;

    public Trackpoint(ITrackpointCreator origin)
    {
        this.origin = origin;
        this.location = origin.GetLocation();
    }

    public void Pass(OldTrain passed)
    {
        if (origin != null)
            origin.Pass(passed);
    }

    public void ConnectNext(Trackpoint _next)
    {
        next = _next;
        distanceToNext = Vector3.Distance(location,next.GetLocation());
    }
    public Vector3 GetLocation()
    {
        return location;
    }

    public Vector2 GetLocation2D()
    {
        return location.To2D();
    }

    public float GetOrientation()
    {
        return orientation;
    }

    public Trackpoint GetNext(bool directionIsForward)
    {
        return Line.GetNextTrackpoint(this, directionIsForward);
    }

    public ITrackpointCreator GetCreator()
    {
        return origin;
    }
}
