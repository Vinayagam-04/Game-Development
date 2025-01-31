using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaMove : MonoBehaviour
{
    public CharacterController cc; // CharacterController for movement
    public float speed = 12f; // Movement speed
    public float minJumpHeight = 1f; // Minimum jump height
    public float maxJumpHeight = 5f; // Maximum jump height
    public float gravity = -9.81f; // Gravity force
    private float verticalVelocity; // Vertical velocity for jumping and falling
    private bool isJumping = false; // Check if the character is jumping
    public float rotationSpeed = 360f; // Speed of rotation
    private Animator anim; // Animator for controlling animations

    void Start()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero; // Initialize moveDirection to zero

        // Handle rotation input
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0); // Rotate left
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0); // Rotate right
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(0, 180 * Time.deltaTime, 0); // Turn backward
        }

        // Handle forward movement
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection = transform.forward * speed; // Move forward in the current direction
        }

        // Handle jumping
        if (cc.isGrounded)
        {
            verticalVelocity = -2f; // Keep grounded
            if (Input.GetKeyDown(KeyCode.J)) // Jump key (J)
            {
                float randomJumpHeight = Random.Range(minJumpHeight, maxJumpHeight); // Random jump height
                verticalVelocity = Mathf.Sqrt(randomJumpHeight * -2f * gravity); // Calculate jump force
                anim.SetTrigger("isJump"); // Trigger jump animation
                isJumping = true; // Mark as jumping
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // Apply gravity when in the air
        }

        // Apply vertical movement (jump or fall)
        moveDirection.y = verticalVelocity;

        // Move the character controller
        cc.Move(moveDirection * Time.deltaTime);

        // Animation control
        if (moveDirection.sqrMagnitude >= 0.1f && cc.isGrounded)
        {
            anim.SetBool("isWalk", true); // Set walk animation
        }
        else
        {
            anim.SetBool("isWalk", false); // Stop walk animation
        }

        // Reset jumping state when grounded
        if (cc.isGrounded && isJumping)
        {
            isJumping = false;
        }
    }
}
