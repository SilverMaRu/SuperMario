using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Item
{
    public float speedJump = 15f;

    private int layer_Ground;
    private int layer_Player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        layer_Ground = LayerMask.NameToLayer("Ground");
        layer_Player = LayerMask.NameToLayer("Player");
        _score = 1000;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = collision.contacts;
        int colliderLayerMask = collision.gameObject.layer;

        if (colliderLayerMask == layer_Ground)
        {
            for (int i = 0; i < contacts.Length; i++)
            {
                if (contacts[i].normal.x * transform.right.x < 0)
                {
                    TurnBreak();
                }
                if (contacts[i].normal.y > 0)
                {
                    Jump();
                }
            }
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    int colliderLayerMask = collision.gameObject.layer;
    //    if (colliderLayerMask == layer_Player)
    //    {
    //        Mario mario = collision.gameObject.GetComponent<Mario>();
    //        if (mario != null)
    //        {
    //            Destroy(gameObject);
    //            mario.OnInvincible();
    //        }
    //    }
    //}

    private void TurnBreak()
    {
        transform.right = -transform.right;
        rgBody2D.velocity = transform.right * speedX;
    }

    private void Jump()
    {
        Vector3 velocity = rgBody2D.velocity;
        velocity = Vector3.Scale(velocity, Vector3.right + Vector3.forward) + (transform.up * speedJump);
        rgBody2D.velocity = velocity;
    }

}
