using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class player : MonoBehaviour
{
    public Animator animator;
    public int high = 10;
    public int speed = 5;
    private Rigidbody2D rb;

    private enum State { idle, running, jumping, falling, hurt }
    private State state = State.idle;
    public int cherries = 0;

    private Collider2D coll;

    private bool isFacingRight = true;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    private float move;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;


    [SerializeField] private float hurt = 10f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private int health = 5;
    [SerializeField] private Text healthAmount;
    [SerializeField] private AudioSource cherrysound;
    [SerializeField] private AudioSource footstep;
    void Start()
    {
             wallJumpingPower = new Vector2(speed, high);
        coll = GetComponent<Collider2D>();
      
        rb = GetComponent<Rigidbody2D>();
     
        animator = GetComponent<Animator>();



        healthAmount.text = health.ToString();
    }


    void Update()
    {
        if(state != State.hurt)
        {
            Input();
            WallSlide();
            WallJump();

            if (!isWallJumping)
            {
                Flip();
            }

        }


        animationState();
        animator.SetInteger("state", (int)state);
    }

    private void Input()
    {
        move = UnityEngine.Input.GetAxis("Horizontal");
        if (move < 0)
        {
            rb.velocity = new Vector2(move * speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        if (move > 0)
        {
            rb.velocity = new Vector2(move * speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {

        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && coll.IsTouchingLayers(ground))
        {
            jump();   
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="collect")
        {
            cherrysound.Play();
            Destroy(collision.gameObject);
            cherries += 1;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                enemy.JumpedOn();

                jump();
            }
            else
            {
                state = State.hurt;
                hadleHealth();
                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurt, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurt, rb.velocity.y);
                }
            }
        }
        
    }

    private void hadleHealth()
    {
        health -= 1;
        healthAmount.text = health.ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }



    private bool IsWalled()
    {
        bool isWalled = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
      
        return isWalled;
    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void WallSlide()
    {
        bool isGrounded = IsGrounded(); // Kiểm tra có chạm đất
        bool isWalled = IsWalled(); // Kiểm tra có bám tường


        if (isWalled && !isGrounded && Mathf.Abs(move) > 0.1f) // Điều kiện này kiểm tra xem nhân vật có đang di chuyển
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }


    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && move < 0f || !isFacingRight && move > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, high);
        state = State.jumping;
    }

    void animationState()
    {


        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) <.1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)  
        {
        
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

    private void foot()
    {
        footstep.Play();
    }

}



   
    


