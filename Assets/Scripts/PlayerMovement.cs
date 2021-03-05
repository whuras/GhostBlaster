using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 15f;
    public bool allowMovement = false;
    Vector3 velocity;

    [Header("Jump")]
    public float gravity = -10f;
    public float jumpHeight = 3f;
    public LayerMask groundMask;
    bool isGrounded;
    bool jumpNow;
    RaycastHit hit;

    [Header("Turn/Camera")]
    public float mouseSensitivity = 100f;
    float mouseX;
    float mouseY;
    float xRotation = 0f;
    Vector3 direction;
    
    CharacterController controller;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        if (allowMovement)
        {
            Movement();
        }
    }

    private void Movement()
    {
        // LOOK WITH MOUSE
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -25f, 25f);
        if(Camera.main != null)
        {
            Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        transform.Rotate(Vector3.up * mouseX);

        // GRAVITY
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1.2f, Color.red);
        isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.2f, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // DIRECTION
        direction = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        // JUMP
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        //}
        gravity = -1000; // jump -disabled

        // MOVE
        velocity.y += gravity * Time.deltaTime;
        controller.Move(direction * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "FloorTrigger")
        {
            FindObjectOfType<FloorController>().FloorTriggered(transform.position);
        }
    }
}
