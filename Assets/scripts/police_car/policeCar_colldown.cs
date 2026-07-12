using System;
using UnityEngine;

public class policeCar_colldown : MonoBehaviour
{
    [SerializeField]
    GameObject policeCar;

    [SerializeField]
    float time;

    void Start()
    {
        policeCar.SetActive(false);
    }

    void Update()
    {
        if (time > 0f)
            time -= Time.deltaTime;
        else
        {
            policeCar.SetActive(true);
            Destroy(gameObject);
        }
    }
}
