using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    public enum Status
    {
        NormalSmall = 0,
        FireSmall = 1,
        NormalBig = 10,
        FireBig = 11
    }
    public Status status = Status.NormalSmall;

    public float inGroundMoveForce = 30;
    public float onSkyMoveForce = 10;

    public float jumpForce = 30;

    private Rigidbody2D rgBody2D;
    private Animator animator;
    private MyPhysics myPhysics;
    
    private float inputTime = 0;
    private float skyInputTime = 0;
    private Vector2 velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myPhysics = GetComponent<MyPhysics>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = myPhysics.velocity;
        //inGround = velocity.y == 0;
        float h = Input.GetAxis("Horizontal");
            float inputDirection = h / Mathf.Abs(h);
        if (h != 0)
        {
            if (inputDirection * velocity.x < 0 || velocity.x == 0)
            {
                inputTime = 0;
            }
            inputTime = Mathf.Min(inputTime + Time.deltaTime * 1f, 10f);
            float finalForce = 0;
            if (!myPhysics.onSky)
            {
                if (h > 0)
                {
                    transform.rotation = Quaternion.identity;
                }
                else if (h < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                finalForce = Mathf.Max(inGroundMoveForce * (Mathf.Clamp(1 / inputTime, 1f, 2f) - 1f), myPhysics.friction);
                //finalForce = inGroundMoveForce;
            }
            else
            {
                finalForce = onSkyMoveForce * (Mathf.Clamp(1 / inputTime, 1f, 2f) - 1f);
                //finalForce = onSkyMoveForce;
            }
            velocity.x += inputDirection * finalForce * Time.deltaTime;
        }

        if (!myPhysics.onSky && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y += jumpForce;
        }

        if (myPhysics.onSky && Input.GetKeyUp(KeyCode.Space) && velocity.y > 0)
        {
            velocity.y *= 0.25f;
        }

        myPhysics.velocity = velocity;

        animator.SetFloat("speed_x", rgBody2D.velocity.x);
        animator.SetFloat("speed_y", rgBody2D.velocity.y);
        animator.SetFloat("force_x", h);
        animator.SetBool("inGround", !myPhysics.onSky);
    }
}
