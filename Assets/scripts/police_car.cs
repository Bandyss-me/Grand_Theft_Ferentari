using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class police_car : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float precisionRange;

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject roadGenerator;

    Graph graph;
    List<Vertex> path=new List<Vertex>();
    bool startCoroutine=true;

    void Start()
    {
        graph = roadGenerator.GetComponent<Graph>();
    }

    Vertex NearestVertex(Vector3 pos)
    {
        Vertex nearest=null;
        float dis = float.MaxValue;
        foreach (Vertex v in graph.vertexes)
        {
            Vector3 realPos = roadGenerator.transform.position + new Vector3(v.pos.x-20f, 1, v.pos.y+20f);
            float l = Vector3.Distance(pos,realPos);
            if (nearest == null)
            {
                nearest = v;
                dis = l;
            }
            else
            {
                if (l < dis)
                {
                    nearest = v;
                    dis = l;
                }
            }
        }
        return nearest;
    }

    IEnumerator GoToPlayer()
    {
        while (true)
        {
            if (!graph.generated)
            { 
                yield return null;
                continue;
            }
            Vertex start = NearestVertex(transform.position);
            Vertex target = NearestVertex(player.transform.position);
            path = graph.FindPath(start, target);
            foreach (Vertex v in path)
            {
                Vector3 realPos = roadGenerator.transform.position + new Vector3(v.pos.x - 20f, 1, v.pos.y + 20f);
                while (Vector3.Distance(transform.position, realPos) > 1f)
                {
                    transform.LookAt(realPos);
                    transform.position += transform.forward * (speed * Time.deltaTime);
                    yield return null;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    void Update()
    {
        if ((player.transform.position - transform.position).magnitude <= 50f)
        {
            StopCoroutine(GoToPlayer());
            startCoroutine = true;
            transform.position += ((player.transform.position - transform.position) * (speed * Time.deltaTime));
        }
        else
        {
            if (startCoroutine == true)
            {
                StartCoroutine(GoToPlayer());
                startCoroutine = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(roadGenerator.transform.position + new Vector3(path[i].pos.x - 20f, 10, path[i].pos.y + 20f), 5);
                if (i > 0)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(roadGenerator.transform.position + new Vector3(path[i].pos.x - 20f, 10, path[i].pos.y + 20f), roadGenerator.transform.position + new Vector3(path[i - 1].pos.x - 20f, 10, path[i - 1].pos.y + 20f));
                }
            }
        }
    }
}
