using UnityEngine;
using System.Collections.Generic;
using System;

public class Conection
{
    public Tuple<Vertex, Vertex> t;
    public float l;

    public Conection(Vertex a, Vertex b)
    {
        t = new Tuple<Vertex, Vertex>(a, b);
        l = Mathf.Sqrt((a.pos.x - b.pos.x) * (a.pos.x - b.pos.x) + (a.pos.y - b.pos.y) * (a.pos.y - b.pos.y));
    }
}
