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
    private Vector2 velocity = Vector2.zero;

    private PhysicsMaterial2D noFriction;
    private PhysicsMaterial2D groundFriction;

    // Start is called before the first frame update
    void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //myPhysics = GetComponent<MyPhysics>();

        noFriction = Resources.Load<PhysicsMaterial2D>("PhysicMaterials/NoFrictionPhysicsMaterial2D");
        groundFriction = Resources.Load<PhysicsMaterial2D>("PhysicMaterials/0.02FrictionPhysicsMaterial2D");
    }

    // Update is called once per frame
    void Update()
    {
        //velocity = myPhysics.velocity;
        velocity = rgBody2D.velocity;
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
            //if (!myPhysics.onSky)
            if (velocity.y == 0)
            {
                if (h > 0)
                {
                    transform.rotation = Quaternion.identity;
                }
                else if (h < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                //finalForce = Mathf.Max(inGroundMoveForce * (Mathf.Clamp(1 / inputTime, 1f, 2f) - 1f), myPhysics.friction);
                finalForce = Mathf.Max(inGroundMoveForce * (Mathf.Clamp(1 / inputTime, 1f, 2f) - 1f), 8.7f);
                
                //finalForce = rgBody2D.sharedMaterial.friction*2 + 10;
                //finalForce = inGroundMoveForce * (Mathf.Clamp(1 / inputTime, 1f, 2f) - 1f, rgBody2D.sharedMaterial.friction);
                //finalForce = inGroundMoveForce;
            }
            else
            {
                finalForce = onSkyMoveForce * (Mathf.Clamp(1 / inputTime, 1f, 2f) - 1f);
                //finalForce = onSkyMoveForce;
            }
            //velocity.x += inputDirection * finalForce * Time.deltaTime;
            rgBody2D.AddForce(Vector2.right * finalForce * inputDirection);
        }

        //if (!myPhysics.onSky && Input.GetKeyDown(KeyCode.Space))
        if (velocity.y == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y += jumpForce;
            rgBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //if (myPhysics.onSky && Input.GetKeyUp(KeyCode.Space) && velocity.y > 0)
        if (velocity.y != 0 && Input.GetKeyUp(KeyCode.Space) && velocity.y > 0)
        {
            //velocity.y *= 0.25f;
            rgBody2D.velocity = Vector2.right * rgBody2D.velocity.x + Vector2.up * rgBody2D.velocity.y * 0.25f;
        }

        //myPhysics.velocity = velocity;
        //rgBody2D.velocity = velocity;

        animator.SetFloat("speed_x", rgBody2D.velocity.x);
        animator.SetFloat("speed_y", rgBody2D.velocity.y);
        animator.SetFloat("force_x", h);
        //animator.SetBool("inGround", !myPhysics.onSky);
        animator.SetBool("inGround", rgBody2D.velocity.y == 0);
        if(rgBody2D.velocity.y == 0)
        {
            rgBody2D.sharedMaterial = groundFriction;
        }
        else
        {
            rgBody2D.sharedMaterial = noFriction;

        }
    }
}
