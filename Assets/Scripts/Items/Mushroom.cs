using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Item
{
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
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int colliderLayerMask = collision.gameObject.layer;
        if (colliderLayerMask == layer_Player)
        {
            Mario mario = collision.gameObject.GetComponent<Mario>();
            if (mario != null)
            {
                Destroy(gameObject);
                mario.StatusChangeTo(Mario.Status.NormalBig);
            }
        }
    }

    private void TurnBreak()
    {
        transform.right = -transform.right;
        rgBody2D.velocity = transform.right * speedX;
    }
}
