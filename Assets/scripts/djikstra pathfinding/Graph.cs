using System;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public List<Vertex> vertexes=new List<Vertex>();
    public List<Conection> connections = new List<Conection>();

    RoadGeneration rgeneration;

    void Start()
    {
        rgeneration = GetComponent<RoadGeneration>();
        rgeneration.GenerateCity();
    }
}
