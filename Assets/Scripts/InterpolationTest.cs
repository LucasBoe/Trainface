using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpolationTest : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float lerp;
    [SerializeField] Transform a, b, c;

    private void OnDrawGizmos()
    {
        Vector3 ab = Vector3.Lerp(a.position,b.position, lerp);
        Vector3 bc = Vector3.Lerp(b.position,c.position, lerp);
        Vector3 ac = Vector3.Lerp(ab, bc, lerp);

        Gizmos.color = Color.white;

        Gizmos.DrawLine(a.position, b.position);
        Gizmos.DrawLine(b.position, c.position);


        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(ab, bc);

        Gizmos.DrawWireSphere(ac, 0.25f);
    }
}
