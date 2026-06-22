using UnityEngine;

public class Vertex
{
    public Vector2Int pos;
    public float cost;
    public Vertex previous;
    public bool visited;

    public Vertex(Vector2Int position)
    {
        pos = position;
        cost = 0;
        previous = null;
        visited = false;
    }
}
