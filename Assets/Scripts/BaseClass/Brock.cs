using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brock : MonoBehaviour
{
    protected Animator animator;

    // Start is called before the first frame update
     protected virtual void Start()
    {
        InitGameObjects();
        InitComponents();
    }

    protected virtual void InitGameObjects() { }

    protected virtual void InitComponents()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void OnHit()
    {
        animator.SetTrigger("hit");
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = collision.contacts;
        for(int i = 0; i < contacts.Length; i++)
        {
            if(contacts[i].normal.y > 0)
            {
                OnHit();
                break;
            }
        }
    }
}
