using System;
using Unity.VisualScripting;
using UnityEngine;

public class testing_script : MonoBehaviour
{
    [SerializeField]
    Transform t1;
    [SerializeField]
    Transform t2;

    void OnDrawGizmos()
    {
        float t=Vector3.Dot(t1.forward, t2.position-t1.position);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(t1.position, t1.forward * t);
        Gizmos.DrawRay(t2.position,t1.position-t2.position);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(t1.position, t1.forward);
    }
    
    float Cal_arm(Vector3 p, Vector3 r)
    {
        float t = Vector3.Dot(p, r);
        return Mathf.Sqrt(Vector3.Magnitude(p - r)*Vector3.Magnitude(p - r)-t*t);
    }
}
