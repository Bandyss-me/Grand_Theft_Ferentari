using UnityEngine;

public class player_raycast : MonoBehaviour
{
    [SerializeField]
    LayerMask carLayer;
    [SerializeField]
    Transform Camera;
    [SerializeField]
    float distance;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance, carLayer) && Input.GetMouseButtonDown(0))
        {
            hit.collider.gameObject.GetComponent<get_in_car>().GetInCar(gameObject, Camera);
        }
    }
}
