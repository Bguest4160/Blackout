using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendTreeAnimationScript : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    float velocityY = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    public float velocityBar = -.05f;
    bool landsoon = true;
    bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    CharacterController controller;
    [SerializeField] Transform landingcheck;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool backPressed = Input.GetKey("s");

        //acceleration
        if (forwardPressed && velocityZ < 0.5f)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (backPressed && velocityZ > -0.5f)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }

        if (leftPressed && velocityX > -0.5f)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        if (rightPressed && velocityX < 0.5f)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //deceleration
        if (!forwardPressed && velocityZ > 0)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        if (!backPressed && velocityZ < 0)
        {
            velocityZ += Time.deltaTime * deceleration;
        }

        if (!leftPressed && velocityX < 0)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        if (!rightPressed && velocityX > 0)
        {
            velocityX -= Time.deltaTime * deceleration;
        }

        //velocity reset
        if (!forwardPressed && !backPressed && velocityZ != 0f && (velocityZ > -0.05 && velocityZ < 0.05))
        {
            velocityZ = 0;
        }

        if (!leftPressed && !rightPressed && velocityX != 0f && (velocityX > -.05 && velocityX < 0.05))
        {
            velocityX = 0;
        }

        //if both directions are clicked
        if (forwardPressed && backPressed)
        {
            if(velocityZ > 0.05)
            {
                velocityZ -= Time.deltaTime * deceleration;
            }
            else if (velocityZ < -0.05)
            {
                velocityZ += Time.deltaTime * deceleration;
            }
        }

        if (leftPressed && rightPressed)
        {
            if (velocityX > 0.05)
            {
                velocityX -= Time.deltaTime * deceleration;
            }
            else if (velocityX < -0.05)
            {
                velocityX += Time.deltaTime * deceleration;
            }
        }

        animator.SetFloat("velocity Z", velocityZ);
        animator.SetFloat("velocity X", velocityX);

        //Jump

        //Debug.Log(landsoon);
        animator.SetBool("landSoon", landsoon);

        if (Input.GetKey("space"))
        {
            animator.SetBool("jump", true);
        }

        else
        {
            animator.SetBool("jump", false);
        }

        velocityY = controller.velocity.y;
        

        if (velocityY < velocityBar)
        {
            animator.SetBool("isFalling", true);
        }

        else
        {
            animator.SetBool("isFalling", false);
        }

        
        landsoon = Physics.CheckSphere(landingcheck.position, 0.6f, ground);

        //animator.SetBool("onGround", isGrounded);



        //Block
        if (Input.GetKey("q"))
        {
            animator.SetBool("block", true);
        }

        else
        {
            animator.SetBool("block", false);
        }


        //punch
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("punch", true);
        }

        else
        {
            animator.SetBool("punch", false);
        }

        if (Input.GetMouseButton(0) || Input.GetKey("q"))
        {
            animator.SetBool("Jump or block", true);
        }

        else
        {
            animator.SetBool("Jump or block", false);
        }

    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "floor")
        {
            animator.SetBool("onGround", true);
        }

        else
        {
            animator.SetBool("onGround", false);
        }
    }
}
