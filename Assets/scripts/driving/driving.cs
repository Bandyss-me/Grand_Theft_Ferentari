using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class driving : MonoBehaviour
{
    [Header("driving")]
    [SerializeField]
    float speed=2.2f;
    [SerializeField]
    float rotationSpeed;

    [Header("Wheels")]
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
    Collider col;

    Vector3 velocity;
    Vector3 torque;
    Rigidbody rb;
    bool isGrounded;
    float wheelRot = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnDrawGizmos()
    {
        if(rb!=null)
            Gizmos.DrawRay(transform.position,rb.linearVelocity*5f);
    }

    void FixedUpdate()
    {
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + transform.up * -0.5f, new Vector3(3.5f, 0.5f, 8f), transform.forward);
        foreach (RaycastHit hit in hits)
        {
            isGrounded = false;
            if (hit.collider != col)
            {
                isGrounded = true;
                break;
            }
        }
        Move();
    }

    void Move()
    {
        if (!isGrounded || isGrounded)
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
            Debug.Log(torque);
            rb.linearVelocity = velocity;
            rb.AddTorque(torque);
            torque = Vector3.zero;
        }
    }
    
    void ApplyTorque()
    {
        ApplyWheelFriction(r1,-600f);
        ApplyWheelFriction(r2,-600f);
        ApplyWheelFriction(r3,-600f);
        ApplyWheelFriction(r4,-600f);
    }

    void ApplyWheelFriction(Transform wheel, float multiplier)
    {
        float lateralF = Vector3.Dot(rb.linearVelocity, wheel.right);
        lateralF=Mathf.Min(Mathf.Max(lateralF,-11770*0.7f),11770*0.7f);
        velocity -= Time.deltaTime*lateralF*wheel.right;
        torque += transform.up * ((wheel.position - transform.position).magnitude * lateralF * multiplier);
    }

    float Cal_arm(Vector3 p, Vector3 r)
    {
        float t = Mathf.Abs(Vector3.Dot(p, r));
        return Mathf.Max(0f,Mathf.Sqrt(Vector3.Magnitude(p - r)*Vector3.Magnitude(p - r)-t*t));
    }
}