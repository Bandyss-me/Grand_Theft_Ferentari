using UnityEngine;

public class get_in_car : MonoBehaviour
{
    public void GetInCar(GameObject player, Transform camera)
    {
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<player_movement>().enabled = false;
        player.transform.position = transform.position+transform.up*1.3f+transform.right*-0.7f+transform.forward*0.25f;
        player.transform.rotation = transform.rotation;
        camera.rotation =Quaternion.Euler(Vector3.forward);
        GetComponent<driving>().enabled = true;
        player.transform.SetParent(transform);
    }

    public void GetOutOfcar(GameObject player)
    {
        player.GetComponent<CapsuleCollider>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<player_movement>().enabled = true;
        player.transform.SetParent(null);
        GetComponent<driving>().enabled = false;
    }
}
