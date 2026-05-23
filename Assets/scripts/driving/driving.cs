using UnityEngine;
using UnityEngine.UIElements;

public class driving : MonoBehaviour
{
    [Header("driving")]
    [SerializeField]
    float speed=2.2f;
    [SerializeField]
    float rotationSpeed;

    [SerializeField]
    Transform r1, r2, r3, r4;

    Vector3 velocity;
    Rigidbody rb;
    bool isGrounded;
    private float wheelRot = 0f;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Physics.BoxCast(transform.position+transform.up*1f, new Vector3(2f,1.2f,3.75f), Vector3.forward, out RaycastHit hit))
        {
            isGrounded = true;
        }
        else isGrounded = false;
        Move();
    }

    void Move()
    {
        if (!isGrounded || isGrounded)
        {
            velocity = rb.linearVelocity;
            if (Input.GetKey(KeyCode.W))
            {
                velocity+=r1.up*speed*-1f*Time.deltaTime;
            }
            if (wheelRot <= 60 && wheelRot >= -60 && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    r1.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
                    r2.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
                    wheelRot += -rotationSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    r1.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                    r2.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                    wheelRot += rotationSpeed * Time.deltaTime;
                }
            }
            else
            {
                if (wheelRot < 0)
                {
                    r1.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                    r2.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                    wheelRot += rotationSpeed * Time.deltaTime;
                }
                else
                {
                    r1.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
                    r2.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
                    wheelRot += -rotationSpeed * Time.deltaTime;
                }
            }
            rb.linearVelocity = velocity;
        }
        ApplyTorqueWheel();
    }

    void ApplyTorqueWheel()
    {
        rb.AddTorque(Cal_arm(transform.position, r1.transform.right*Vector3.Dot(r1.transform.right, rb.linearVelocity)*-1f)  *  (r1.transform.right*Vector3.Dot(r1.transform.right, rb.linearVelocity)*-1f));
        rb.AddTorque(Cal_arm(transform.position, r2.transform.right*Vector3.Dot(r2.transform.right, rb.linearVelocity)*-1f)  *  (r2.transform.right*Vector3.Dot(r2.transform.right, rb.linearVelocity)*-1f));
    }

    float Cal_arm(Vector3 p, Vector3 r)
    {
        float t = Mathf.Abs(Vector3.Dot(p, r));
        return Mathf.Sqrt(Vector3.Magnitude(p - r)*Vector3.Magnitude(p - r)-t*t);
    }
}
