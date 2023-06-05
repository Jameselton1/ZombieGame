using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    // running speed
    public float speed = 12f;
    // speed at which the player falls
    public float gravity = -9.81f;
    // the height to which the player jumps
    public float jumpHeight = 3f;
    
    // this gameobject is used to detect when the player is standing on something.
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // used to give falling a more realistic feel, rather than falling at a constant speed you gain momentum.
    Vector3 velocity;
    // check is the player grounded.
    bool isGrounded;


    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if(isGrounded && velocity.y < 0){ 
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    } 
}
