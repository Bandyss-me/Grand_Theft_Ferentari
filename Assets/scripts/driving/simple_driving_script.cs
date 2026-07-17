using System;
using System.Linq;
using UnityEngine;

public class simple_driving_script : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] 
    float speed=5f;
    [SerializeField] 
    float rotationSpeed;
    
    [SerializeField]
    Transform r1, r2, r3, r4, steeringWheel;
    
    [Header("Ground Checking")]
    [SerializeField]
    Collider[] col;
    [Tooltip("Put the main ground collider as the first one")]
    
    bool isGrounded;
    float wheelRot = 0f;

    void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapBox(
            col[0].bounds.center,
            col[0].bounds.extents,
            col[0].transform.rotation
        );
        isGrounded = false;
        foreach (Collider hit in hits)
        {
            isGrounded = false;
            if (!col.Contains(hit))
            {
                isGrounded = true;
                break;
            }
        }
        Move();
    }

    void Move()
    {
        if (isGrounded)
        {
            float input=0f;
            if (Input.GetKey(KeyCode.W))
            {
                input+=1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                input -= 1f;
            }
            if (wheelRot <= 60 && wheelRot >= -60 && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    r1.Rotate(Vector3.forward * (-rotationSpeed * Time.deltaTime));
                    r2.Rotate(Vector3.forward * (-rotationSpeed * Time.deltaTime));
                    steeringWheel.Rotate(Vector3.forward * (-rotationSpeed * 2f * Time.deltaTime));
                    wheelRot += -rotationSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    r1.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
                    r2.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
                    steeringWheel.Rotate(Vector3.forward * (rotationSpeed * 2f * Time.deltaTime));
                    wheelRot += rotationSpeed * Time.deltaTime;
                }
            }
            else
            {
                if (wheelRot < 0)
                {
                    r1.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
                    r2.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
                    steeringWheel.Rotate(Vector3.forward * (rotationSpeed * 2f * Time.deltaTime));
                    wheelRot += rotationSpeed * Time.deltaTime;
                }
                else
                {
                    r1.Rotate(Vector3.forward * (-rotationSpeed * Time.deltaTime));
                    r2.Rotate(Vector3.forward * (-rotationSpeed * Time.deltaTime));
                    steeringWheel.Rotate(Vector3.forward * (-rotationSpeed * 2f * Time.deltaTime));
                    wheelRot += -rotationSpeed * Time.deltaTime;
                }
            }
            transform.position += transform.forward * (input * speed * Time.deltaTime);
            Steering();
        }
    }

    void Steering()
    {
        transform.Rotate(Vector3.up * (wheelRot * Time.deltaTime));
    }
}
