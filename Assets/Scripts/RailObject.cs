using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class RailObject : MonoBehaviour
{
    [SerializeField] float baseSpeed;
    [SerializeField] Vector3 rotationOffset;
    [SerializeField] Vector3 positionOffset;
    Trackpoint trackpoint;

    [Button]
    private void Rail ()
    {
        trackpoint = Rails.GetClosestPoint(transform.position);
        if (trackpoint != null)
        {
            transform.position = trackpoint.GetLocation() + positionOffset;
            AdaptTrackpointOrientation();
        } else
        {
            Debug.LogError("Object could not be railed");
        }
    }

    [Button]
    private void StartDriving()
    {
        StartCoroutine(DriveCoroutine());
    }

    private IEnumerator DriveCoroutine()
    {
        while (trackpoint.GetNext() != null)
        {
            float distancePerFrame = Time.deltaTime * baseSpeed;

            if (Vector3.Distance(transform.position, trackpoint.GetNext().GetLocation() + positionOffset) < distancePerFrame)
            {
                trackpoint = trackpoint.GetNext();
                AdaptTrackpointOrientation();
            }

            transform.position = Vector3.MoveTowards(transform.position,trackpoint.GetNext().GetLocation() + positionOffset, distancePerFrame);
            yield return null;
        }
    }
    private void AdaptTrackpointOrientation ()
    {
        Debug.Log("adapt to orientation: " + trackpoint.GetOrientation());
        transform.localRotation = Quaternion.Euler(rotationOffset.x, trackpoint.GetOrientation() + rotationOffset.y, rotationOffset.z);
    }
}
