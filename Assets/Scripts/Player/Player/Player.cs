using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] private float movepSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpTime = 0.5f;

    [Header("Facing")]
    [SerializeField] public bool isFacingRight;

    [Header("Ground Check")]
    [SerializeField] private float extraHeight = 0.25f;
    [SerializeField] LayerMask groundLayer;
    
    [Header("RigidBody")]
    private Rigidbody2D rb;
    private Collider2D coll;

    [Header("Values and Bools")]
    private float moveInput;
    private bool isJumping;
    private bool isWalking;
    private float jumpTimeCounter;

    private RaycastHit2D groundHit;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        DirectionCheck();
    }

    private void Update()
    {
        Move();
        Jump();

    }

    #region Movement Functions
    private void Move()
    {
        moveInput = UserInput.instance.moveInput.x;
        if (moveInput > 0 || moveInput < 0) 
        {
            TurnCheck();
        }
        rb.velocity = new Vector2(moveInput * 5f, rb.velocity.y);
    }
    #endregion


    #region Turn Check Functions
    private void DirectionCheck() 
    {
        if (isFacingRight && moveInput < 0f || isFacingRight && moveInput > 0f) 
        {
            isFacingRight = !isFacingRight;
            Vector2 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }

    private void TurnCheck() 
    {
        if (UserInput.instance.moveInput.x > 0f && !isFacingRight) 
        {
            Turn();
        }
        else if (UserInput.instance.moveInput.x < 0f && isFacingRight) 
        {
            Turn();
        }
    }

    private void Turn() 
    {
        if (isFacingRight) 
        {
            Vector3 rotate = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotate);
            isFacingRight = !isFacingRight;
        }
        else
        {
            Vector3 rotate = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotate);
            isFacingRight = !isFacingRight; 
        }
    }
    #endregion

    #region Jump Functions
    private void Jump() 
    {
        //if jump button was pressed
        if (UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame() && IsGrounded()) 
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        //if jump button was being held
        if (UserInput.instance.controls.Jumping.Jump.IsPressed()) 
        {
            if (isJumping && jumpTimeCounter > 0) 
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime; //amount of time nince last rendered
            }
            else 
            {
                isJumping = false;
            }
        }
        //if jump button was released
        if(UserInput.instance.controls.Jumping.Jump.WasReleasedThisFrame()) 
        {
            isJumping = false;
        }
        DrawGroundCheck();
    }
    #endregion

    #region Ground Checker
    private bool IsGrounded() 
    {
        groundHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, extraHeight, groundLayer);

        if (groundHit.collider != null) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }




    #endregion

    #region Debug Functions
    private void DrawGroundCheck() 
    {
        Color rayColor;
        
        if (IsGrounded()) 
        {
            rayColor = Color.green;
        }
        else 
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(coll.bounds.center + new Vector3(coll.bounds.extents.x, 0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(coll.bounds.center - new Vector3(coll.bounds.extents.x, 0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(coll.bounds.center - new Vector3(coll.bounds.extents.x, coll.bounds.extents.y + extraHeight), Vector2.right * (coll.bounds.extents.x * 2f), rayColor);
    }
    #endregion
}


