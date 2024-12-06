using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public Animator animator;
    public int high = 10;
    public int speed = 5;
    private Rigidbody2D rb;
    private float leftright;
    private enum State { idle, running, jumping, falling, hurt }
    private State state = State.idle;
    public int cherries = 0;

    private Collider2D coll;
    [SerializeField] private float hurt = 10f;
    [SerializeField] private LayerMask ground; 
    void Start()
    {
     
        coll = GetComponent<Collider2D>();
      
        rb = GetComponent<Rigidbody2D>();
     
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if(state != State.hurt)
        {
            Input();

        }
        

        animationState();
        animator.SetInteger("state", (int)state);
    }

    private void Input()
    {
        float move = UnityEngine.Input.GetAxis("Horizontal");
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
}



   
    


