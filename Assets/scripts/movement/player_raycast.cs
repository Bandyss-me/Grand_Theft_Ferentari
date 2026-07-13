using UnityEngine;

public class player_raycast : MonoBehaviour
{
    [SerializeField]
    LayerMask carLayer;
    [SerializeField]
    Transform Camera;
    [SerializeField]
    float distance;

    bool inCar = false;
    get_in_car Get_in_car_script;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance) && hit.collider.gameObject.name=="DaciaLogan" && Input.GetMouseButtonDown(0))
        {
            Get_in_car_script=hit.collider.gameObject.GetComponent<get_in_car>();
            Get_in_car_script.GetInCar(gameObject, Camera);
            inCar = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && inCar == true)
        {
            
            Get_in_car_script.GetOutOfCar(gameObject);
            Get_in_car_script = null;
            inCar = false;
        }
    }
}
