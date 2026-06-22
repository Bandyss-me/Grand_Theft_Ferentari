using System;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public List<Vertex> vertexes=new List<Vertex>();
    public List<Conection> connections = new List<Conection>();

    RoadGeneration rgeneration;
    
    List<Vertex> path=new List<Vertex>();
    List<Vertex> visited=new List<Vertex>();
    List<Vertex> unvisited = new List<Vertex>();

    void Start()
    {
        rgeneration = GetComponent<RoadGeneration>();
        rgeneration.GenerateCity();
    }

    bool CheckConnection(Conection x)
    {
        foreach (Conection c in connections)
        {
            if (c.t.Item1 == x.t.Item1 && c.t.Item2 == x.t.Item2)
            {
                return true;
            }
        }

        return false;
    }

    void DoNeighbours(Vertex v)
    {
        foreach (Vertex current in vertexes)
        {
            Conection c = new Conection(v, current);
            if (CheckConnection(c))
            {
                if (unvisited.Contains(current) || visited.Contains(current))
                {
                    if (current.cost > v.cost + c.l)
                    {
                        current.cost=v.cost + c.l;
                        current.previous = v;
                    }
                }
                else
                {
                    unvisited.Add(current);
                    current.cost = v.cost + c.l;
                    current.previous = v;
                }
            }
        }
    }

    List<Vertex> TransformToPath(Vertex target)
    {
        List<Vertex> path = new List<Vertex>();
        Vertex current = target;
        while (current.previous!=null)
        {
            path.Add(current);
            current = current.previous;
        }
        foreach (Vertex v in vertexes)
        {
            v.previous = null;
            v.cost = 0;
            v.visited = false;
        }
        path.Reverse();
        return path;
    }

    public List<Vertex> FindPath(Vertex start, Vertex target)
    {
        visited = new List<Vertex>();
        unvisited = new List<Vertex>();
        unvisited.Add(start);
        int i = 0;
        while (vertexes.Count > visited.Count)
        {
            i++;
            Vertex next = null;
            foreach (Vertex v in unvisited)
            {
                if (next == null)
                    next = v;
                else next = (next.cost <= v.cost) ? next : v;
            }
            visited.Add(next);
            DoNeighbours(next);
            unvisited.Remove(next);
            if (i > 10000)
            {
                Debug.LogError("Too many iterations!");
                break;
            }
        }
        return TransformToPath(target);
    }
}
