using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speedX = 3f;
    public float delayDestroyTime = 0.5f;

    protected Rigidbody2D rgBody2D;
    protected Animator animator;
    protected Collider2D[] childrenscoll2D;

    protected Vector2 velocity;
    protected int layer_Ground;
    protected int layer_Player;
    protected int layer_PlayerBullet;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        childrenscoll2D = transform.GetComponentsInChildren<Collider2D>();
        animator = transform.GetComponentInChildren<Animator>();
        rgBody2D.velocity = transform.right * speedX;

        layer_Ground = LayerMask.NameToLayer("Ground");
        layer_Player = LayerMask.NameToLayer("Player");
        layer_PlayerBullet = LayerMask.NameToLayer("PlayerBullet");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = collision.contacts;
        int colliderLayerMask = collision.gameObject.layer;

        if (colliderLayerMask == layer_PlayerBullet)
        {
            OnHit();
        }
        else if (colliderLayerMask == layer_Ground)
        {
            for (int i = 0; i < contacts.Length; i++)
            {
                if (contacts[i].normal.x * transform.right.x < 0)
                {
                    TurnBreak();
                }
            }
        }
        else if (colliderLayerMask == layer_Player)
        {
            for (int i = 0; i < contacts.Length; i++)
            {
                if (contacts[i].normal.y < 0)
                {
                    StepedOn();
                }
            }
        }
    }

    protected virtual void TurnBreak()
    {
        transform.right = -transform.right;
        rgBody2D.velocity = transform.right * speedX;
    }

    protected virtual void StepedOn()
    {
        animator.SetTrigger("stepedOn");
        rgBody2D.velocity = Vector2.zero;
    }

    protected virtual void OnHit()
    {
        animator.SetTrigger("onHit");
        rgBody2D.velocity = Vector2.right * 2 + Vector2.up * 10;
    }

    protected virtual void Die()
    {
        rgBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        for(int i = 0; i < childrenscoll2D.Length; i++)
        {
            childrenscoll2D[i].enabled = false;
        }
        Destroy(gameObject, delayDestroyTime);
    }
}
