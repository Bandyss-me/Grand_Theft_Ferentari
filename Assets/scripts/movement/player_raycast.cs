using System;
using UnityEngine;

public class player_raycast : MonoBehaviour
{
    [SerializeField]
    Transform Camera;
    [SerializeField]
    float distance;

    public player_data data;

    bool inCar = false;
    get_in_car Get_in_car_script;

    void LateUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
        {
            if (hit.collider.gameObject.name == "DaciaLogan")
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Get_in_car_script=hit.collider.gameObject.GetComponent<get_in_car>();
                    Get_in_car_script.GetInCar(gameObject, Camera);
                    inCar = true;
                }
            }
            else if (hit.collider.gameObject.name == "pedestrian(Clone)")
            {
                if (Input.GetMouseButtonDown(1))
                {
                    data.money+= hit.collider.gameObject.GetComponent<pedestrian_script>().RobMe(gameObject);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    hit.collider.gameObject.GetComponent<pedestrian_health_script>().HitMe(5f, hit.collider.gameObject.transform.position-transform.position);
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E) && inCar == true)
        {
            Get_in_car_script.GetOutOfCar(gameObject);
            Get_in_car_script = null;
            inCar = false;
        }
    }
}
