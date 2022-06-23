using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public float speed = 12;
    public float jumpheight = 3;
    public float mouseSen = 100;
    public Transform cam;
    public CharacterController controller;
    public float gravity = -1;

    public Transform groundCheck;
    public float groundDist = 0.4f;
    public LayerMask groundMask;        //how to tell what is ground or not

    float xRot = 0;     //Basically how much we are looking, up or down, in degrees.
    Vector3 velocity;
    bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   //Keep cursor on the screen, at the dead center.
    }

    // Update is called once per frame
    void Update()
    {
        MouseControls();
        MoveControls();
        ShootControls();
    }

    void MoveControls()
    {
        // Check in a small area around the ground check point to see if there is a collider marked "ground" or not
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
     
        if (isGrounded && velocity.y < 0)
        {
            // Makes sure player actually lands, not hovers a little above the ground
            // This also prevents the downward velocity from increasing forever
            velocity.y = -2;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");            //Rememver that Y is up/down, not forward/back. So this is Z.

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2 * gravity);     //Physics or something idk
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);     //using time.deltatime twice power of 2 for fall accelleration.

    }

    void MouseControls()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSen * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSen * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -85, 85);                      //don't flip upside down
        cam.localRotation = Quaternion.Euler(xRot, 0, 0);

        transform.Rotate(Vector3.up, mouseX);
    }

    void ShootControls()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit ray;
            if(Physics.Raycast(cam.position, cam.forward, out ray, Mathf.Infinity))
            {
                Destroy(ray.collider.gameObject);
            }
        }
    }

}
