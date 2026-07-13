using System;
using UnityEngine;

public class player_raycast : MonoBehaviour
{
    [SerializeField]
    Transform Camera;
    [SerializeField]
    float distance;
    [SerializeField] 
    GameObject lclick;

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
                lclick.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    Get_in_car_script=hit.collider.gameObject.GetComponent<get_in_car>();
                    Get_in_car_script.GetInCar(gameObject, Camera);
                    inCar = true;
                }
            }
            else if (hit.collider.gameObject.name == "pedestrian(Clone)")
            {
                lclick.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    data.money+= hit.collider.gameObject.GetComponent<pedestrian_script>().RobMe();
                }
            }
            else lclick.SetActive(false);
        }
        else lclick.SetActive(false);
        
        if (Input.GetKeyDown(KeyCode.E) && inCar == true)
        {
            Get_in_car_script.GetOutOfCar(gameObject);
            Get_in_car_script = null;
            inCar = false;
        }
    }
}
