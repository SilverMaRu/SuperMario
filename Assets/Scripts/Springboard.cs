using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springboard : MonoBehaviour
{
    public GameObject springTarget;
    public int springNum = 1;
    public float springSpeed = 5f;

    private int layer_SpringTarget;
    private Collider2D coll2D;

    // Start is called before the first frame update
    void Start()
    {
        layer_SpringTarget = springTarget.layer;
        coll2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == layer_SpringTarget)
        {
            Rigidbody2D rgBody2D = collision.GetComponent<Rigidbody2D>();
            Vector2 velocity = rgBody2D.velocity;
            rgBody2D.velocity = Vector2.right * velocity.x + Vector2.up * springSpeed;
            springNum--;
        }
        if (springNum <= 0)
        {
            coll2D.enabled = false;
        }
    }
}
