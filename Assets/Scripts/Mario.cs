using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

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
    //[Range(0, 20)]
    public float maxHorizontalSpeed = 10;
    //[Range(0, 50)]
    public float jumpForce = 35;
    public float maxVerticalSpeed = 20;

    private bool isAlive = true;
    // 起跳后上升中标示
    private bool isRising = false;
    // 下坠中标示
    private bool isFalling = false;
    private bool inGround = false;


    private SpriteRenderer sr;
    private Rigidbody2D rgBody2D;
    private Animator animator;

    //// 提升至最高速度用时
    //private float amountTime = 4;
    //// 已用时间
    //private float moveTime = 0;
    private float startTime = 0;
    private float twoPi = Mathf.PI * 2;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rgBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rgBody2D.velocity.y < 0.1 && rgBody2D.velocity.y > -0.1)
        {
            inGround = true;
        }
        else
        {
            inGround = false;
        }

        float h = Input.GetAxis("Horizontal");
        float velocity_x = rgBody2D.velocity.x;
        if (h != 0)
        {
            rgBody2D.gravityScale = 1;
            float direction = h / Mathf.Abs(h);
            //if(velocity_x<0.1||velocity_x > -0.1)
            //{
            //    velocity_x = direction * 1.5f;
            //}
            //velocity_x = velocity_x + Mathf.Clamp(h + 0.15f * h / Mathf.Abs(h)*Time.deltaTime, -0.5f, 0.5f);
            //velocity_x = velocity_x + 0.2f * h / Mathf.Abs(h);
            //velocity_x = velocity_x + Mathf.Clamp(0.2f / h, -0.5f, 0.5f);
            //velocity_x = velocity_x + 15 * Mathf.Clamp(100f / h, -1f, 1f) * Time.deltaTime;
            //velocity_x = velocity_x + 1f / h;
            //velocity_x = velocity_x + direction * 3 * maxHorizontalSpeed / 2 * Time.deltaTime;
            //velocity_x = h * maxHorizontalSpeed;
            if (velocity_x == 0)
            {
                velocity_x = 0.7f * direction;
            }
            else
            {
                velocity_x += 0.15f * direction;
            }
        }
        else
        {
            rgBody2D.gravityScale = 15;
        }

        //if (h > 0)
        //{
        //    rgBody2D.gravityScale = 1;
        //    if (velocity_x == 0)
        //    {
        //        velocity_x = 0.7f;
        //    }
        //    else
        //    {
        //        velocity_x += 0.15f;
        //    }
        //}
        //else if (h < 0)
        //{
        //    rgBody2D.gravityScale = 1;
        //    if (velocity_x == 0)
        //    {
        //        velocity_x = -0.7f;
        //    }
        //    else
        //    {
        //        velocity_x -= 0.15f;
        //    }
        //}
        //else
        //{
        //    rgBody2D.gravityScale = 15;
        //}
        velocity_x = Mathf.Clamp(velocity_x, -maxHorizontalSpeed, maxHorizontalSpeed);
        rgBody2D.velocity = Vector2.right * velocity_x + Vector2.up * rgBody2D.velocity;
        if (inGround)
        {
            if (h > 0)
            {
                transform.rotation = Quaternion.identity;
            }
            else if (h < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            animator.SetFloat("speed_x", velocity_x);
            animator.SetFloat("force_x", h);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rgBody2D.gravityScale = 1;
                Vector2 finalJumpForce = Vector2.up * jumpForce;
                rgBody2D.AddForce(finalJumpForce, ForceMode2D.Impulse);
                ChangeToRising();
            }
        }
        else
        {
            rgBody2D.gravityScale = 10;
        }

        if (isRising && Input.GetKeyUp(KeyCode.Space))
        {
            float velocity_y = rgBody2D.velocity.y;
            velocity_y = Mathf.Clamp(velocity_y, -maxVerticalSpeed, maxVerticalSpeed * 0.5f);
            rgBody2D.velocity = Vector2.right * rgBody2D.velocity + Vector2.up * velocity_y;
            ChangeToFalling();
        }

        if (isFalling)
        {
            float velocity_y = rgBody2D.velocity.y;
            velocity_y = Mathf.Clamp(velocity_y, -maxVerticalSpeed, maxVerticalSpeed);
            rgBody2D.velocity = Vector2.right * rgBody2D.velocity + Vector2.up * velocity_y;
        }

        if (animator.GetBool("inGround") != inGround)
        {
            animator.SetBool("inGround", inGround);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = collision.contacts;
        for (int i = 0; i < contacts.Length; i++)
        {
            if (!inGround && contacts[i].normal.y < 0)
            {
                ChangeToFalling();
                rgBody2D.velocity = Vector2.zero;
            }
        }
    }

    public void ChangeToRising()
    {
        isRising = true;
        isFalling = false;
    }

    public void ChangeToFalling()
    {
        isRising = false;
        isFalling = true;
    }
}
