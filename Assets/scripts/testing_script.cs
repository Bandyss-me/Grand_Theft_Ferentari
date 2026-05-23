using System;
using Unity.VisualScripting;
using UnityEngine;

public class testing_script : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward*5);
        Gizmos.color = Color.blue;
        Vector3 velocity = new Vector3(2, 0, 4);
        Gizmos.DrawRay(transform.position, velocity);
        float t = Vector3.Dot(transform.right, velocity);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.right*t);
        Gizmos.color = Color.deepPink;
        Gizmos.DrawRay(transform.position, transform.right*t*-1f);
    }
}
