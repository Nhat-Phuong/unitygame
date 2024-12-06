using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected AudioSource deathsound;

    // Start is called before the first frame update
   protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        deathsound = GetComponent<AudioSource>();
    }

  
    public void JumpedOn()
    {
        anim.SetTrigger("death");
        deathsound.Play();
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;

    }
    private void death()
    {
        Destroy(this.gameObject);
    }
}
