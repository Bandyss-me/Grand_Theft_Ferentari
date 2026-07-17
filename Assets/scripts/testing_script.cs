using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class testing_script : MonoBehaviour
{
    Graph pathFinding;
    List<Vertex> path;
    
    void OnDrawGizmos()
    {
       Gizmos.color = Color.green;
       if (path != null)
       {
           foreach (Vertex v in path)
           {
               Gizmos.DrawSphere(transform.position + new Vector3(v.pos.x - 10f, 10f, v.pos.y + 10f), 5f);
           }
           Debug.LogError(path.Count);
       }
    }

    void Start()
    {
        pathFinding = GetComponent<Graph>();
        if (pathFinding != null)
        {
            int i, j;
            Random rand = new Random();
            i = rand.Next(pathFinding.vertexes.Count);
            j = rand.Next(pathFinding.vertexes.Count);
            path = pathFinding.FindPath(pathFinding.vertexes[i], pathFinding.vertexes[j]);
        }
    }
}
