using System;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class driving : MonoBehaviour
{
    [Header("driving")]
    [SerializeField]
    float speed=2.2f;
    [SerializeField]
    float rotationSpeed;

    [Header("Wheels")] [SerializeField]
    float grip = 1f;
    [Space]
    
    [SerializeField]
    Transform r1;
    [SerializeField]
    Transform r2;
    [SerializeField]
    Transform r3;
    [SerializeField]
    Transform r4;

    [Header("Ground Checking")]
    [SerializeField]
    Collider[] col;
    [Tooltip("Put the main ground collider as the first one")]

    Vector3 velocity;
    Vector3 torque;
    Rigidbody rb;
    bool isGrounded;
    float wheelRot = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

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
            velocity = rb.linearVelocity;
            if (Input.GetKey(KeyCode.W))
            {
                velocity+=r4.up * (speed * -1f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                velocity+=r4.up * (speed * 0.6f * Time.deltaTime);
            }
            if (wheelRot <= 60 && wheelRot >= -60 && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    r1.Rotate(Vector3.forward * (-rotationSpeed * Time.deltaTime));
                    r2.Rotate(Vector3.forward * (-rotationSpeed * Time.deltaTime));
                    wheelRot += -rotationSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    r1.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
                    r2.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
                    wheelRot += rotationSpeed * Time.deltaTime;
                }
            }
            else
            {
                if (wheelRot < 0)
                {
                    r1.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
                    r2.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
                    wheelRot += rotationSpeed * Time.deltaTime;
                }
                else
                {
                    r1.Rotate(Vector3.forward * (-rotationSpeed * Time.deltaTime));
                    r2.Rotate(Vector3.forward * (-rotationSpeed * Time.deltaTime));
                    wheelRot += -rotationSpeed * Time.deltaTime;
                }
            }
            ApplyTorque();
            rb.linearVelocity = velocity;
            rb.AddTorque(torque);
            torque = Vector3.zero;
        }
    }
    
    void ApplyTorque()
    {
        ApplyWheelFriction(r1);
        ApplyWheelFriction(r2);
        ApplyWheelFriction(r3);
        ApplyWheelFriction(r4);
    }

    void ApplyWheelFriction(Transform wheel)
    {
        Vector3 wheelVel = rb.GetPointVelocity(wheel.position);
        float lateralF = Vector3.Dot(wheelVel, wheel.right);
        lateralF = Mathf.Clamp(lateralF, -11770 * 0.7f, 11770 * 0.7f);
        rb.AddForceAtPosition(-wheel.right*lateralF*0.7f,wheel.position);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            UnStuck();
        }
    }

    void UnStuck()
    {
        transform.position = transform.up * 5f;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}