using UnityEngine;

public class Vertex
{
    public Vector2Int pos;
    public int connections;

    public Vertex(Vector2Int pos)
    {
        pos = pos;
        connections = 0;
    }
}
