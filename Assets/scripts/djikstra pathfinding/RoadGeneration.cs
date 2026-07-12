using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class RoadGeneration : MonoBehaviour
{
    [SerializeField]
    GameObject intersection_prefab, road_prefab, pedestrian_prefab;
    
    [SerializeField]
    Vector2Int cellSpacing;

    [SerializeField]
    GameObject[] cells;

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
        SmoothGrid(3);
    }
    
    public void GenerateCity()
    {
        graph = GetComponent<Graph>();
        GenerateForm();
        CreateGrid();
    }
    
    

    Vertex CheckVertex(Vertex x)
    {
        foreach (Vertex v in graph.vertexes)
        {
            if (v.pos == x.pos)
                return v;
        }
        return null;
    }
    
    bool CheckConnection(Conection x)
    {
        foreach (Conection c in graph.connections)
        {
            if (c.t.Item1 == x.t.Item1 && c.t.Item2 == x.t.Item2)
            {
                return true;
            }
        }
        return false;
    }

    Vector3 VertexToPos(Vertex v)
    {
        return transform.position + new Vector3(v.pos.x, 1, v.pos.y);
    }

    void CreateGrid()
    {
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                if (cityForm[i, j] == true)
                {
                    List<Vertex> l=new List<Vertex>();
                    l.Add(new Vertex(new Vector2Int((i)*cellSpacing.x,(j)*cellSpacing.y)));
                    l.Add(new Vertex(new Vector2Int((i+1)*cellSpacing.x,(j)*cellSpacing.y)));
                    l.Add(new Vertex(new Vector2Int((i)*cellSpacing.x,(j+1)*cellSpacing.y)));
                    l.Add(new Vertex(new Vector2Int((i+1)*cellSpacing.x,(j+1)*cellSpacing.y)));

                    int pedestrians=UnityEngine.Random.Range(5,5);
                    while (pedestrians-->0)
                    {
                        GameObject pedestrian = Instantiate(pedestrian_prefab ,transform.position+new Vector3(l[0].pos.x, 1, l[0].pos.y),Quaternion.identity,transform);
                        pedestrian.GetComponent<pedestrian_script>().waypoints = new Vector3[]{VertexToPos(l[0])+new Vector3(-10f,0,30f),VertexToPos(l[1])+new Vector3(-40f,0,30f),VertexToPos(l[2])+new Vector3(-40f,0,0),VertexToPos(l[3])+new Vector3(-10f,0,10f)};
                    }
                    
                    for(int q=0;q<4;q++)
                    {
                        Vertex k = CheckVertex(l[q]);
                        if (k==null)
                        {
                            graph.vertexes.Add(l[q]);
                            Instantiate(intersection_prefab, transform.position + new Vector3(l[q].pos.x, 1, l[q].pos.y), Quaternion.identity, transform);
                            int cellIndex = UnityEngine.Random.Range(0, cells.Length);
                            Vector3 offset = new Vector3(-20,0,20);
                            Instantiate(cells[cellIndex], transform.position + new Vector3(l[q].pos.x+cellSpacing.x/2, 1, l[q].pos.y+cellSpacing.y/2)+offset,Quaternion.identity,transform);
                        }
                        else{
                            l[q]=k;
                        }
                    }
                    for (int x = 0; x < 4; x++)
                    {
                        for (int y = 0; y < 4; y++)
                        {
                            if(x==y || x==0 && y==3 || x==1 && y==2 || x==3 && y==0 || x==2 && y==1)
                                continue;
                            if (!CheckConnection(new Conection(l[x], l[y])))
                            {
                                graph.connections.Add(new Conection(l[x],l[y]));
                                if (!CheckConnection(new Conection(l[y], l[x])))
                                {
                                    if (l[x].pos.x == l[y].pos.x)
                                    {
                                        for (int n = Math.Min(l[x].pos.y, l[y].pos.y)+40; n <= Math.Max(l[x].pos.y, l[y].pos.y)-20; n += 20)
                                        {
                                            Instantiate(road_prefab, transform.position+new Vector3(l[x].pos.x, 1f, n),Quaternion.Euler(0,0,0),transform);
                                        }
                                    }
                                    else
                                    {
                                        for (int n = Math.Min(l[x].pos.x, l[y].pos.x); n < Math.Max(l[x].pos.x, l[y].pos.x)-40; n += 20)
                                        {
                                            Instantiate(road_prefab, transform.position+new Vector3(n, 1f, l[x].pos.y),Quaternion.Euler(0,90,0),transform);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        graph.generated = true;
    }
}
