using UnityEngine;

public class playerscript : MonoBehaviour
{
    public Rigidbody rb;

    public float movSpeed = 2000f;
    public float jumpHeight = 2000f;
    //public float rotSpeed = 500f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            rb.transform.position += transform.forward * movSpeed * Time.deltaTime;
        }

        else if (Input.GetKey("s"))
        {
            rb.transform.position -= transform.forward * movSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            rb.transform.position -= transform.right * movSpeed * Time.deltaTime;
        }

        else if (Input.GetKey("d"))
        {
            rb.transform.position += transform.right * movSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown("space"))
        {
            rb.AddForce(0, jumpHeight * Time.deltaTime, 0);
        }
    }
}

/* if (Input.GetKey("a"))
        {
            rb.transform.Rotate(0, -rotSpeed * Time.deltaTime, 0);
        }

        else if (Input.GetKey("d"))
        {
            rb.transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
        }
*/