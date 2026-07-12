using UnityEngine;
using System.Collections.Generic;

public class cell_culling : MonoBehaviour
{
    [SerializeField] 
    float distance;
    
    public List<GameObject> cells = new List<GameObject>();
    
    public Transform player;

    void LateUpdate()
    {
        foreach (GameObject gb in cells)
        {
            if (Vector3.Distance(player.position, gb.transform.position)>distance)
                gb.SetActive(false);
            else gb.SetActive(true);
        }
    }
}
