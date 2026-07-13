using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class police_car : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float precisionRange;
    
    public GameObject player;
    public GameObject roadGenerator;

    Graph graph;
    List<Vertex> path=new List<Vertex>();
    private Coroutine _coroutine;

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
                yield return new WaitForSeconds(0.1f);
                continue;
            }
            Vertex start = NearestVertex(transform.position);
            Vertex target = NearestVertex(player.transform.position);
            if (start == target)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }
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
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position,player.transform.position-transform.position, out hit,50f) && hit.collider.gameObject==player)
        {
            if (hit.distance < 5f)
            {
                player.GetComponent<data_saving>().Save();
                SceneManager.LoadScene(3);
            }

            if (_coroutine != null)
            {
                 StopCoroutine(_coroutine);
                 _coroutine = null;
            }
            Vector3 pPos = new Vector3(player.transform.position.x, 1f, player.transform.position.z);
            transform.LookAt(pPos);
            transform.position += ((pPos-transform.position).normalized * (speed * Time.deltaTime));
        }
        else
        {
            if (_coroutine==null)
            {
                _coroutine=StartCoroutine(GoToPlayer());
            }
        }
    }
}
