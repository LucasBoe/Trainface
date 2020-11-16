using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rails : MonoBehaviour
{
    static List<Trackpoint> trackpoints = new List<Trackpoint>();
    private void Start()
    {
        Trackpoint before = null;

        foreach (Transform t in transform)
        {
            Trackpoint newTrackpoint = new Trackpoint(t.position,before,t.rotation.eulerAngles.y);
            before?.ConnectNext(newTrackpoint);
            trackpoints.Add(newTrackpoint);
            before = newTrackpoint;
            Debug.Log("added new trackpoint");
        }
    }

    public static Trackpoint GetClosestPoint(Vector3 position)
    {
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

public class Trackpoint
{
    Vector3 location;
    Trackpoint before;
    Trackpoint next;
    float orientation;
    float distanceToNext;
    public Trackpoint (Vector3 _location, Trackpoint _before, float _orientation)
    {
        location = _location;
        before = _before;
        orientation = _orientation;
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

    public float GetOrientation()
    {
        return orientation;
    }

    public Trackpoint GetNext()
    {
        return next;
    }

}
