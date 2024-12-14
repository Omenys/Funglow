using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player_Movement : MonoBehaviour
{
    public float speed;
    private float Move;
    private Rigidbody2D rb;
    public float jump;
    public bool isJumping;
    public bool isGrounded;
    private int maxJumps = 2;
    private int jumpsLeft;
    private bool isFacingRight = true;
    private Animator animator;
    private TrailRenderer trailRenderer;

    [Header("Dashing")]
    [SerializeField] private float dashingVelocity = 14;
    [SerializeField] private float dashingTime = 0.5f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsLeft = maxJumps;
        animator = GetComponent<Animator>();
        trailRenderer = GetComponent <TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Player movement 
        Move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(speed * Move, rb.velocity.y);
        Flip();

        // Jump input
        if (Input.GetButtonDown("Jump") && (isJumping == false || jumpsLeft > 0))
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump));
            isGrounded = false;
            jumpsLeft -= 1;
        }

        // Animation 
        animator.SetBool(name: "isWalking", Move != 0);

        var dashInput = Input.GetButtonDown("Dash");

        // Starting dash 
        if (dashInput && canDash)
        {
            isDashing = true;
            canDash = false;
            trailRenderer.emitting = true;
            dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (dashingDir == Vector2.zero)
            {
                dashingDir = new Vector2(transform.localScale.x, 0);
            }

            // Stopping dash
            StartCoroutine(StopDashing());
        }


        if (isDashing)
        {
            rb.velocity = dashingDir.normalized * dashingVelocity;
            return;
        }
        if (isGrounded == true)
        {
            canDash = true;
        }

    }
    private void Flip()
    {
        // Flip sprite to look opposite direction
        if (isFacingRight && Move < 0f || !isFacingRight && Move > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Colliding with Ground object
        if (!other.gameObject.CompareTag("Ground"))
        {
            return;
        }

        isJumping = false;
        isGrounded = true;
        jumpsLeft = maxJumps;
        Debug.Log("Stopped jumping");

        
        /*
        // Colliding with Ground object
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            isGrounded = true;
            jumpsLeft = maxJumps;
            Debug.Log("touched ground");
        }
        */
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // Check if we are transitioning off of the ground.
        if (!other.gameObject.CompareTag("Ground"))
        {
            return;
        }

        Debug.Log("Started jumping.");
        isGrounded = false;
        isJumping = true;


        /*
        // Transitioned off Ground object
        if (other.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isJumping = true;
            isGrounded = false;
            Debug.Log("left ground");
        } 
        */
    }


    private IEnumerator StopDashing()
    {
        // Dashing ability
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        isDashing = false;
    }
    
   
   
}
