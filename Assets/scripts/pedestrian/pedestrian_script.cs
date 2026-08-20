using UnityEngine; 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class pedestrian_script : MonoBehaviour
{
    public Coroutine coroutine;
    public Vector3[] waypoints;
    public RoadGeneration roadGenerator;

    [SerializeField] 
    float min_speed = 3f, max_speed = 6f;

    [SerializeField] 
    float min_run_speed = 8f, max_run_speed = 16f;

    [SerializeField]
    int police_spawn_chance=3;

    [SerializeField] 
    float min_money = 3f, max_money = 150f;

    GameObject player;
    Vector3 offset;
    float speed,run_speed;
    int i = 0;
    bool direction, robbed, initialized;
    Animator animatior;
    CharacterController _controller;

    void Start()
    {
        animatior = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        Spawn();
    }

    public void Spawn()
    {
        speed = UnityEngine.Random.Range(min_speed, max_speed);
        run_speed = UnityEngine.Random.Range(min_run_speed, max_run_speed);
        offset = new Vector3(UnityEngine.Random.Range(2f, 8f), 0, UnityEngine.Random.Range(2f, 8f));
        direction = UnityEngine.Random.value > 0.5f;
        SpawnRandomly();
        coroutine=StartCoroutine(walk());
        initialized=true;
        robbed = false;
    }

    private void OnEnable()
    {
        if(!initialized)
            return;
        SpawnRandomly();
        coroutine = StartCoroutine(walk());
    }

    IEnumerator walk()
    {
        while (true)
        {
            while (Vector3.Distance(transform.position, waypoints[i] + offset) > 3f)
            {
                transform.LookAt(waypoints[i]+offset);
                Vector3 movement = transform.forward * speed;
                movement.y -= 9.8f;
                _controller.Move(movement * Time.deltaTime);
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
        i = (direction) ? l : k;
        if (waypoints[k].x == waypoints[l].x)
        {
            float z = UnityEngine.Random.Range(Mathf.Min(waypoints[k].z, waypoints[l].z),Mathf.Max(waypoints[k].z, waypoints[l].z));
            _controller.enabled = false;
            transform.position=new Vector3(waypoints[k].x,waypoints[k].y,z);
            _controller.enabled = true;
        }
        else
        {
            float x = UnityEngine.Random.Range(Mathf.Min(waypoints[k].x, waypoints[l].x),Mathf.Max(waypoints[k].x, waypoints[l].x));
            _controller.enabled = false;
            transform.position=new Vector3(x,waypoints[k].y,waypoints[k].z);
            _controller.enabled = true;
        }
    }

    IEnumerator run()
    {
        yield return new WaitForSeconds(3.2f);
        RaycastHit hit;
        while (Vector3.Distance(transform.position, player.transform.position)<60f || Physics.Raycast(transform.position,player.transform.position-transform.position, out hit,200f) && hit.collider.gameObject==player)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            Vector3 movement = transform.forward * run_speed;
            movement.y -= 9.8f;
            _controller.Move(movement * Time.deltaTime);
            if (Vector3.Distance(transform.position, player.transform.position) < 4f)
            {
                player.GetComponent<data_saving>().Save();
                SceneManager.LoadScene(3);
            }
            yield return null;
        }
        animatior.SetTrigger("lost");
        coroutine = StartCoroutine(walk());
        robbed = false;
        yield break;
    }
    
    public float RobMe(GameObject thief)
    {
        player = thief;
        if (robbed)
        {
            return 0;
        }
        robbed = true;
        animatior.SetTrigger("robbed");
        StopCoroutine(coroutine);
        coroutine = StartCoroutine(run());
        if (UnityEngine.Random.Range(0, police_spawn_chance) == 0)
        {
            roadGenerator.SpawnAPoliceCar();
        }
        return (float)System.Math.Round(UnityEngine.Random.Range(min_money, max_money),0);
    }
}
