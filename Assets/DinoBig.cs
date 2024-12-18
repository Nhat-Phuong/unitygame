using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoBig : Enemy
{
    [SerializeField] private float left; // Vị trí giới hạn bên trái
    [SerializeField] private float right; // Vị trí giới hạn bên phải
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float jump = 5f;


    private bool leftface = true; // Xác định hướng di chuyển
    private Collider2D coll;

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();

    }

    private void Update()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < .1)
            {
                anim.SetBool("falling", true);
                anim.SetBool("jumping", false);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);

        }

    }
    private void move()
    {
        if (leftface)
        {
            if (transform.position.x > left)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(5, 5, 5); // Xoay mặt sang phải
                }
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-speed, jump);
                    anim.SetBool("jumping", true);
                }
            }
            else
            {
                leftface = false; // Đổi hướng
            }
        }
        else
        {
            if (transform.position.x < right)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-5, 5, 5); // Xoay mặt sang trái
                }
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(speed, jump);
                    anim.SetBool("jumping", true);

                }
            }
            else
            {
                leftface = true; // Đổi hướng
            }
        }
    }



}
