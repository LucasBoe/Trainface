using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rails : MonoBehaviour
{
    [SerializeField] Transform RailElementPrefab;
    [SerializeField] List<Trackpoint> trackpoints = new List<Trackpoint>();

    Line line;

    public void Init(Line line)
    {
        this.line = line;
        line.OnChangeLength += UpdateTracks;
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
                    Instantiate(RailElementPrefab, Vector2.Lerp(first, second, (float)i / length).To3D(), rotation, transform);
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

    private void CreateTrackpointsFromChilds()
    {
        Trackpoint before = null;

        foreach (Transform t in transform)
        {
            Trackpoint newTrackpoint = new Trackpoint(t.position,before,t.rotation.eulerAngles.y, line);
            before?.ConnectNext(newTrackpoint);
            trackpoints.Add(newTrackpoint);
            before = newTrackpoint;
            Debug.Log("added new trackpoint");
        }
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
    [SerializeField] City city;
    [SerializeField] Vector3 location;
    [SerializeField] Trackpoint before;
    [SerializeField] Trackpoint next;
    [SerializeField] float orientation;
    float distanceToNext;

    public Line Line;

    public Trackpoint(City _city, Trackpoint _before, float _orientation, Line line)
    {
        city = _city;
        location = _city.transform.position;
        before = _before;
        orientation = _orientation;
        Line = line;
    }

    public Trackpoint (Vector3 _location, Trackpoint _before, float _orientation, Line line)
    {
        location = _location;
        before = _before;
        orientation = _orientation;
        Line = line;
    }

    public void Pass(Train passed)
    {
        if (city != null)
            city.Pass(passed);
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

    public Trackpoint GetNext(bool directionIsForward)
    {
        return directionIsForward?next:before;
    }

}
