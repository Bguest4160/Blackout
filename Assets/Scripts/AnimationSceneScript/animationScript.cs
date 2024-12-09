using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationScript : MonoBehaviour
{

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        { 
            animator.SetBool("isRunningF", true);
        }

        else
        {
            animator.SetBool("isRunningF", false);
        }

        if (Input.GetKey("s"))
        {
            animator.SetBool("isRunningB", true);
        }

        else
        {
            animator.SetBool("isRunningB", false);
        }

        if (Input.GetKey("a"))
        {
            animator.SetBool("isRunningL", true);
        }

        else
        {
            animator.SetBool("isRunningL", false);
        }

        if (Input.GetKey("d"))
        {
            animator.SetBool("isRunningR", true);
        }

        else
        {
            animator.SetBool("isRunningR", false);
        }

        if (Input.GetMouseButton(0))
        {
            animator.SetBool("punch", true);
        }

        else
        {
            animator.SetBool("punch", false);
        }

        if (Input.GetKey("space"))
        {
            animator.SetBool("jump", true);
        }

        else
        {
            animator.SetBool("jump", false);
        }

        if (Input.GetKey("q"))
        {
            animator.SetBool("block", true);
        }

        else
        {
            animator.SetBool("block", false);
        }
    }
}
