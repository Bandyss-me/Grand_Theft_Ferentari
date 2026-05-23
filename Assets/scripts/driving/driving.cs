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
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Physics.BoxCast(transform.position+transform.up*1f, new Vector3(2f,1.2f,3.75f), Vector3.forward, out RaycastHit hit))
        {
            Debug.Log("Grounded");
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
            /*if (r1.rotation.eulerAngles.y <= 60 && r1.rotation.eulerAngles.y >= -60)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        r1.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
                        r2.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
                    }

                    if (Input.GetKey(KeyCode.D))
                    {
                        r1.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                        r2.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    if (!(r1.eulerAngles.y <= 1 && r1.eulerAngles.y >= -1))
                    {
                        if (r1.eulerAngles.y >= -60)
                        {
                            r1.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                            r2.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                        }
                        else if (r2.eulerAngles.y >= -60)
                        {
                            r1.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
                            r2.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
                        }
                    }
                }
            }*/
            rb.linearVelocity = velocity;
        }
    }

    void ApplyDriftingBak()
    {
        if (isGrounded)
        {
            
        }
    }

    float Cal_arm(Vector3 p, Vector3 r)
    {
        float t = Vector3.Dot(p, r);
        return Mathf.Sqrt(Vector3.Magnitude(p - r)*Vector3.Magnitude(p - r)-t*t);
    }
}
