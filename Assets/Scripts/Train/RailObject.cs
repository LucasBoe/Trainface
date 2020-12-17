using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class RailObject : MonoBehaviour
{
    [SerializeField] bool directionIsForward;
    public float baseSpeed;
    [SerializeField] Vector3 rotationOffset;
    [SerializeField] Vector3 positionOffset;
    Trackpoint trackpoint;

    Coroutine driving;
    public void Rail (Line line)
    {
        trackpoint = line.Rails.GetClosestTrackpoint(transform.position);
        if (trackpoint != null)
        {
            transform.position = trackpoint.GetLocation() + positionOffset;
            AdaptTrackpointOrientation();
        } else
        {
            Debug.LogError("Object could not be railed");
        }

        if (driving == null)
            StartDriving();
    }

    [Button]
    protected void StartDriving()
    {
        StartCoroutine(DriveCoroutine());
    }

    public void StopDriving()
    {
        Destroy(gameObject);
    }

    protected virtual void TryPass (Trackpoint passed)
    {
        //
    }

    private IEnumerator DriveCoroutine()
    {
        while (trackpoint != null && (trackpoint.GetNext(true) != null || trackpoint.GetNext(false) != null))
        {
            if (trackpoint.GetNext(directionIsForward) != null)
            {

                float distancePerFrame = (Time.deltaTime * baseSpeed) / trackpoint.Line.Length;

                if (Vector3.Distance(transform.position, trackpoint.GetNext(directionIsForward).GetLocation() + positionOffset) < distancePerFrame)
                {
                    trackpoint = trackpoint.GetNext(directionIsForward);

                    if (trackpoint != null)
                    {
                        AdaptTrackpointOrientation();
                        TryPass(trackpoint);
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, trackpoint.GetNext(directionIsForward).GetLocation() + positionOffset, distancePerFrame);
                    yield return null;
                }
            } else
            {
                directionIsForward = !directionIsForward;
                AdaptTrackpointOrientation();
            }
        }

        Debug.LogError(trackpoint.GetNext(false) + " < " + trackpoint + " > " + trackpoint.GetNext(true));

        StopDriving();
    }
    private void AdaptTrackpointOrientation ()
    {
        if (trackpoint.GetNext(directionIsForward) == null)
        {
            transform.localRotation = Quaternion.Euler(rotationOffset.x, trackpoint.GetOrientation() + rotationOffset.y, rotationOffset.z);
        }
        else
        {
            Vector3 tpCurrent = trackpoint.GetLocation();
            Vector3 tpNext = trackpoint.GetNext(directionIsForward).GetLocation();

            Debug.Log("adapt to orientation: " + trackpoint.GetOrientation());
            transform.localRotation = Quaternion.Euler(Quaternion.LookRotation((tpCurrent - tpNext).normalized, Vector3.up).eulerAngles + rotationOffset); //Quaternion.Euler(rotationOffset.x, trackpoint.GetOrientation() + rotationOffset.y + (directionIsForward?0f:0f), rotationOffset.z);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(trackpoint.GetLocation(),1);
        Gizmos.color = Color.yellow;
        if (trackpoint.GetNext(directionIsForward) != null)
        Gizmos.DrawWireSphere(trackpoint.GetNext(directionIsForward).GetLocation(), 1);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, trackpoint.GetOrientation(), 0) * Vector3.forward);
    }
}
