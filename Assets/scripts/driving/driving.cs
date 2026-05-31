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
    Transform r1, r2, r3, r4;

    [Header("Ground Checking")]
    [SerializeField]
    Collider col;

    Vector3 velocity;
    Rigidbody rb;
    bool isGrounded;
    float wheelRot = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = (!isGrounded)?Color.red:Color.green;
        Gizmos.DrawCube(transform.position+transform.up*-0.5f, new Vector3(3.5f,0.5f,8f));
    }

    void FixedUpdate()
    {
        if (Physics.BoxCast(transform.position+transform.up*-0.5f, new Vector3(3.5f,0.5f,8f), transform.forward, out RaycastHit hit))
        {
            isGrounded = true;
        }
        else isGrounded = false;
        //Debug.Log(isGrounded);
        Move();
    }

    void Move()
    {
        if (!isGrounded || isGrounded)
        {
            velocity = rb.linearVelocity;
            if (Input.GetKey(KeyCode.W))
            {
                velocity+=r1.up * (speed * -1f * Time.deltaTime);
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
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void ApplyTorque()
    {
        ApplyWheelFriction(r1);
        ApplyWheelFriction(r2);
        ApplyWheelFriction(r3);
        ApplyWheelFriction(r4);
    }

    void ApplyWheelFriction(Transform wheel)
    {
        float lateralF = Vector3.Dot(rb.linearVelocity, wheel.right);
        lateralF=Mathf.Min(lateralF,11770*0.7f);
        velocity -= Time.deltaTime*lateralF*wheel.right;
        rb.AddTorque(transform.up * ((wheel.position-transform.position).magnitude * lateralF));
        Debug.Log(transform.up * ((wheel.position-transform.position).magnitude * lateralF));
    }

    float Cal_arm(Vector3 p, Vector3 r)
    {
        float t = Mathf.Abs(Vector3.Dot(p, r));
        return Mathf.Max(0f,Mathf.Sqrt(Vector3.Magnitude(p - r)*Vector3.Magnitude(p - r)-t*t));
    }
}
