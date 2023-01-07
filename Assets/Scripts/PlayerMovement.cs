using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 3f;

    [SerializeField] public Transform groundCheck_1;
    [SerializeField] public Transform groundCheck_2;
    [SerializeField] public Transform groundCheck_3;

    Rigidbody2D rb;
    Animator anim;

    static bool isFacingRight = true;

    float horizontal;

    bool isGrounded = false;
    bool isJumping;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Physics2D.Linecast(transform.position, groundCheck_1.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, groundCheck_2.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, groundCheck_3.position, 1 << LayerMask.NameToLayer("Ground")))
        {   // Touching the ground
            isGrounded = true;
            isJumping = false;
        } else
        {
            // In Mid Air
            isGrounded = false;
            isJumping = true;
        }

        Move();

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            Jump(); 
        }
    }

    private void Move()
    {
        // Input X AXIS
        horizontal = Input.GetAxisRaw("Horizontal");

        float moveX = horizontal * speed * Time.fixedDeltaTime;

        // Move Player
        Vector2 horizontalMovement = new Vector2(moveX, rb.velocity.y);
        rb.velocity = horizontalMovement;

        if(isGrounded) 
        {
            // As long as horizontal movement is not zero
            // Then player is moving and play the walk animation
            anim.SetFloat("Speed", Mathf.Abs(horizontal));
        }

        // Player Jump Anim
        anim.SetBool("IsJumping", isJumping);

        Flip(horizontal);
    }

    void Flip(float horizontal)
    {
        // Flip the sprite based on the direction of movement
        if (horizontal > 0)
        {
            // Right
            isFacingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontal < 0)
        {
            // Left
            isFacingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        } 
    }

    void Jump()
    {
        // Push Player Upward
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public static bool getIsFacingRight()
    {
        return isFacingRight;
    }

}
