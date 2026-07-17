using System;
using System.Linq;
using UnityEngine;

public class driving_script : MonoBehaviour
{
    [Header("driving")]
    [SerializeField]
    float speed=30f;
    [SerializeField]
    float rotationSpeed;
    [SerializeField] 
    float maxSpeed=80f;

    [Header("Wheels")] [SerializeField]
    float grip = 80f;

    [Space] [SerializeField] 
    Transform r1, r2, r3, r4, steeringWheel;

    [Header("Ground Checking")]
    [SerializeField]
    float floatingDis;

    Rigidbody rb;
    bool isGrounded;
    float wheelRot = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, 0.5f, 0);
        rb.angularDamping = 1f;
    }

    void FixedUpdate()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, floatingDis);
        isGrounded = false;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.transform.IsChildOf(transform))
                continue;
            isGrounded = true;
            rb.AddForce(transform.up * (floatingDis-hit.distance), ForceMode.Acceleration);
        }
        Move();
    }

    void Move()
    {
        if (isGrounded)
        {
            float input = 0f;
            if (Input.GetKey(KeyCode.W))
            {
                input += 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                input -= 1;
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                wheelRot = Mathf.MoveTowards(wheelRot, 0f, rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                wheelRot = Mathf.MoveTowards(wheelRot, -60f, rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                wheelRot = Mathf.MoveTowards(wheelRot, 60f, rotationSpeed * Time.deltaTime);
            }
            else wheelRot = Mathf.MoveTowards(wheelRot, 0f, rotationSpeed * Time.deltaTime);
            
            r1.localRotation = Quaternion.Euler(0, 0, wheelRot);
            r2.localRotation = Quaternion.Euler(0, 0, wheelRot);
            steeringWheel.localRotation = Quaternion.Euler(-156f, 0, wheelRot);

            rb.AddForce(transform.forward * (input * speed), ForceMode.Acceleration);
            if (rb.linearVelocity.magnitude > maxSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            ApplyTorque();
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
        lateralF = Mathf.Clamp(lateralF, -11770 * grip, 11770 * grip);
        rb.AddForceAtPosition(-wheel.right * (lateralF * grip),wheel.position, ForceMode.Acceleration);
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
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position += transform.up * 0.2f;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}