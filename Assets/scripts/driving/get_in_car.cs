using UnityEngine;

public class get_in_car : MonoBehaviour
{
    public void GetInCar(GameObject player, Transform camera)
    {
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<player_movement>().enabled = false;
        player.transform.position = transform.position+transform.up*1.3f+transform.right*-0.7f+transform.forward*0.25f;
        GetComponent<simple_driving_script>().enabled = true;
        player.transform.SetParent(transform);
        player.transform.localRotation = Quaternion.Euler(0,0,0);
        camera.localRotation = Quaternion.Euler(0,0,0);
    }

    public void GetOutOfCar(GameObject player)
    {
        player.transform.position = player.transform.position + player.transform.right * -2f;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.GetComponent<CapsuleCollider>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<player_movement>().enabled = true;
        player.transform.SetParent(null);
        GetComponent<simple_driving_script>().enabled = false;
    }
}
