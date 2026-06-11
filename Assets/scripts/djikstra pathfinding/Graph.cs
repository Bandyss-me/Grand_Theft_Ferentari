using System;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    public Vector2Int gridMaxSize;

    public List<Vertex> vertexes=new List<Vertex>();
    public Dictionary<Tuple<Vertex, Vertex>, bool> connections=new Dictionary<Tuple<Vertex, Vertex>, bool>();
}
