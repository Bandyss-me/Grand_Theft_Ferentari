using UnityEngine;

public class driving : MonoBehaviour
{
    [Header("driving")]
    [SerializeField]
    float speed=2.2f;
    [SerializeField]
    float rotationSpeed;

    [SerializeField]
    Transform r1, r2, r3, r4;

    [SerializeField]
    float floating_dis;

    Vector3 velocity;
    Rigidbody rb;
    bool isGrounded;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position-transform.up*0.1f, Vector3.down, out RaycastHit hit, floating_dis))
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
            if (Input.GetKey(KeyCode.W))
            {
                velocity+=r1.forward*speed;
            }
            if (r1.rotation.eulerAngles.y <= 60 && r1.rotation.eulerAngles.y >= -60)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        r1.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
                        r2.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
                    }

                    if (Input.GetKey(KeyCode.D))
                    {
                        r1.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
                        r2.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    if (!(r1.eulerAngles.y <= 1 && r1.eulerAngles.y >= -1))
                    {
                        if (r1.eulerAngles.y >= -60)
                        {
                            r1.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
                            r2.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
                        }
                        else if (r2.eulerAngles.y >= -60)
                        {
                            r1.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
                            r2.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
                        }
                    }
                }
            }
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
