using System;
using System.Linq;
using UnityEngine;

public class driving_script : MonoBehaviour
{
    [Header("driving")]
    [SerializeField]
    float speed=2.2f;
    [SerializeField]
    float rotationSpeed;

    [SerializeField] 
    float maxSpeed=50f;

    [Header("Wheels")] [SerializeField]
    float grip = 1f;

    [Space] [SerializeField] 
    Transform r1, r2, r3, r4, steeringWheel;

    [Header("Ground Checking")]
    [SerializeField]
    Collider[] col;
    [Tooltip("Put the main ground collider as the first one")]

    Rigidbody rb;
    bool isGrounded;
    float wheelRot = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, 0.2f, 0);
        rb.angularDamping = 1f;
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

            rb.AddForce(transform.forward * (input * speed));
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
        /*float steeringSlip = 0;
        if (wheel == r1 || wheel == r2)
        {
            steeringSlip = Vector3.Dot(wheel.forward, rb.linearVelocity);
        }
        lateralF += steeringSlip;*/
        lateralF = Mathf.Clamp(lateralF, -11770 * 0.7f, 11770 * 0.7f);
        rb.AddForceAtPosition(-wheel.right * (lateralF * 0.7f),wheel.position);
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