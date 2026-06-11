using System;
using System.IO;
using UnityEngine;

public class RoadGeneration : MonoBehaviour
{
    [SerializeField]
    Vector2Int cellSpacing;

    public bool[,] cityForm=new bool[30,30];

    Graph graph;

    int GetNeighbours(int i, int j)
    {
        int n = 0;
        for (int x = i - 1; x <= i + 1; x++)
        {
            for (int y = j - 1; y <= j + 1; y++)
            {
                if(x-i==0 && y-j==0) continue;
                if (x < 0 || x >= 30 || y < 0 || y >= 30) continue;
                n += (cityForm[x, y] == true) ? 1:0;
            }
        }

        return n;
    }
    
    void SmoothGrid(int times)
    {
        for (; times > 0; times--)
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    int neighbours = GetNeighbours(i,j);
                    if (neighbours >= 5) cityForm[i, j] = true;
                    else if (neighbours <= 3) cityForm[i, j] = false;
                }
            }
        }
    }
    
    void GenerateForm()
    {
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                int chance = Mathf.RoundToInt(Mathf.Sqrt((i-15)*(i-15)+(j-15)*(j-15)))/2;
                cityForm[i,j]=(UnityEngine.Random.Range(0,chance)==0);
            }
        }
        SmoothGrid(2);
    }
    
    void Start()
    {
        graph = GetComponent<Graph>();
        GenerateForm();
        CreateGrid();
    }

    void DebugCityForm()
    {
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                File.AppendAllText(@"/home/andy/Grand_Theft_Ferentari/Assets/scripts/djikstra pathfinding/debug.txt",(cityForm[i,j]==true)?"1 ":"  "); 
            }
            File.AppendAllText(@"/home/andy/Grand_Theft_Ferentari/Assets/scripts/djikstra pathfinding/debug.txt","\n");
        }
    }

    Vertex UniqueVertex(Vertex x)
    {
        foreach (Vertex v in graph.vertexes)
        {
            if (v.pos == x.pos)
            {
                return v;
            }
        }
        return null;
    }

    void CreateGrid()
    {
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                if (cityForm[i, j] == true)
                {
                    Vertex A = new Vertex(new Vector2Int((i)*cellSpacing.x,(j)*cellSpacing.y));
                    Vertex B = new Vertex(new Vector2Int((i-1)*cellSpacing.x,(j)*cellSpacing.y));
                    Vertex C = new Vertex(new Vector2Int((i)*cellSpacing.x,(j-1)*cellSpacing.y));
                    Vertex D = new Vertex(new Vector2Int((i-1)*cellSpacing.x,(j-1)*cellSpacing.y));
                    
                }
            }
        }
    }
}
