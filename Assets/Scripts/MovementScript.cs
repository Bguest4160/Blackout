using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MovementScript : NetworkBehaviour 
{
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform otherCamera;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float Speed = 6.0f;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -30f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    public float jumpHeight = 6f;
    float velocityY;
    public bool isGrounded;
    bool isWaiting;

    float cameraCap;
    float cameraCap2;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);
    }

    void Update()
    {
        UpdateMouse();
        UpdateMove();
    }

    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraCap -= currentMouseDelta.y * mouseSensitivity;

        cameraCap = Mathf.Clamp(cameraCap, -80.0f, 80.0f);
        cameraCap2 = Mathf.Clamp(cameraCap, -80.0f, 80.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraCap;
        otherCamera.localEulerAngles = Vector3.right * cameraCap2;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocityY -= gravity * -2.0f * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Speed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        //jump bit

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (isGrounded! && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        }
    }
    public void ChangeSpeedStats()
    {
        Speed = 50f;
        Invoke("ResetSpeedStats", 5f);
    }

    public void ChangeJumpStats()
    {
        jumpHeight = 50f;
        Invoke("ResetJumpStats", 5f);
    }

    public void ResetSpeedStats()
    {
        Speed = 6f;
    }

    public void ResetJumpStats()
    {
        jumpHeight = 6f;
    }

    public void ChangeBigStats()
    {
        transform.localScale = new Vector3(2f, 2f, 2f);
        Invoke("ResetBigStats", 10f);
    }

    public void ResetBigStats()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void ChangeSmallStats()
    {
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        Invoke("ResetSmallStats", 10f);
    }

    public void ResetSmallStats()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
  
    public void ChangeHealStats()
    {
        
    }
}
