using UnityEngine; 
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class pedestrian_script : MonoBehaviour
{
    public Vector3[] waypoints;
    public RoadGeneration roadGenerator;

    [SerializeField] 
    float min_speed = 3f, max_speed = 6f;

    [SerializeField]
    int police_spawn_chance=3;

    [SerializeField] 
    float min_money = 3f, max_money = 150f;
    
    Vector3 offset;
    float speed;
    int i = 0;
    bool direction;

    void Start()
    {
        speed = UnityEngine.Random.Range(min_speed, max_speed);
        offset = new Vector3(UnityEngine.Random.Range(1f, 9f), 0, UnityEngine.Random.Range(1f, 5f));
        direction = Random.value > 0.5f;
        SpawnRandomly();
        StartCoroutine(walk());
    }

    IEnumerator walk()
    {
        while (true)
        {
            while (Vector3.Distance(transform.position, waypoints[i] + offset) > 10f)
            {
                transform.LookAt(waypoints[i]);
                transform.position += transform.forward * (speed * Time.deltaTime);
                yield return null;
            }
            i = (direction) ? i + 1 : i - 1;
            if (i < 0) i = 3;
            else if (i > 3) i = 0;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void SpawnRandomly()
    {
        int k = UnityEngine.Random.Range(0, 4);
        int l = k + 1;
        if (l>3) l = 0;
        Debug.LogError(l);
        if (waypoints[k].x == waypoints[l].x)
        {
            float z = UnityEngine.Random.Range(Mathf.Min(waypoints[k].z, waypoints[l].z),Mathf.Max(waypoints[k].z, waypoints[l].z));
            transform.position=new Vector3(waypoints[k].x,waypoints[k].y,z);
        }
        else if (waypoints[k].z == waypoints[l].z)
        {
            float x = UnityEngine.Random.Range(Mathf.Min(waypoints[k].x, waypoints[l].x),Mathf.Max(waypoints[k].x, waypoints[l].x));
            transform.position=new Vector3(x,waypoints[k].y,waypoints[k].z);
        }
        i = (direction) ? l : k;
    }

    public float RobMe()
    {
        if (UnityEngine.Random.Range(0, police_spawn_chance) == 0)
        {
            roadGenerator.SpawnAPoliceCar();
        }
        return UnityEngine.Random.Range(min_money, max_money);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, waypoints[i] + new Vector3(0, 2f, 0));
        Gizmos.DrawCube(waypoints[i] + new Vector3(0, 2f, 0), Vector3.one);
    }
}
