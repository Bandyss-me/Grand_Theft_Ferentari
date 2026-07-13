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

    void LateUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance) && hit.collider.gameObject.name=="DaciaLogan" && Input.GetMouseButtonDown(0))
        {
            Get_in_car_script=hit.collider.gameObject.GetComponent<get_in_car>();
            Get_in_car_script.GetInCar(gameObject, Camera);
            inCar = true;
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, distance) && hit.collider.gameObject.name == "pedestrian(Clone)" && Input.GetMouseButtonDown(0))
        {
            hit.collider.gameObject.GetComponent<pedestrian_script>().RobMe();
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, distance) && Input.GetMouseButtonDown(0))
        {
            Debug.LogError(hit.collider.gameObject.name);
        }
        if (Input.GetKeyDown(KeyCode.E) && inCar == true)
        {
            Get_in_car_script.GetOutOfCar(gameObject);
            Get_in_car_script = null;
            inCar = false;
        }
    }
}
