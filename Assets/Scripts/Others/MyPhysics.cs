using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class MyPhysics : MonoBehaviour
{
    public float friction = 50;
    public float gravity = 50;
    public Vector2 velocity = Vector2.zero;

    public bool onSky { get; private set; } = true;

    private Rigidbody2D rgBody2D;
    private Collider2D coll2D;

    // Start is called before the first frame update
    void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onSky)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else if (!onSky && velocity.x != 0)
        {
            float direction = velocity.x / Mathf.Abs(velocity.x);
            velocity.x -= direction * friction * Time.deltaTime;
            if (velocity.x * direction < 0)
            {
                velocity.x = 0;
            }
        }
        rgBody2D.velocity = velocity;
    }

    private void FixedUpdate()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = collision.contacts;
        for (int i = 0; i < contacts.Length; i++)
        {
            // 撞墙
            if (Mathf.Abs(contacts[i].normal.x) > 0.5f && contacts[i].normal.x * velocity.x < 0)
            {
                velocity.x = 0;
            }

            // 着地
            if (onSky && contacts[i].normal.y > 0)
            {
                velocity.y = 0;
                onSky = false;
            }
            else if (onSky && contacts[i].normal.y < 0)
            {
                velocity.y = 0;
                onSky = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = collision.contacts;
        for (int i = 0; i < contacts.Length; i++)
        {
            // 撞墙
            if (Mathf.Abs(contacts[i].normal.x) > 0.5f && contacts[i].normal.x * velocity.x < 0)
            {
                velocity.x = 0;
            }

            // 着地
            if (onSky && contacts[i].normal.y > 0)
            {
                velocity.y = 0;
                onSky = false;
            }
            else if (onSky && contacts[i].normal.y < 0)
            {
                velocity.y = 0;
                onSky = true;
            }
            Debug.DrawLine(contacts[i].point, contacts[i].point + contacts[i].normal * 0.3f, Color.white);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onSky = true;
    }
}
