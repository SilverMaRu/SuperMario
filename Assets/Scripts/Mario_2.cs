using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class Mario_2 : MonoBehaviour
{
    public enum Status
    {
        NormalSmall = 0,
        FireSmall = 1,
        NormalBig = 10,
        FireBig = 11
    }

    public Status status = Status.NormalSmall;
    
    public float startHorizontalSpeed = 1;
    public float maxHorizontalSpeed = 5;
    
    public float startJumpSpeed = 25;
    public float maxVerticalSpeed = 20;

    public float gravity = 50;
    private Vector2 velocity = Vector2.zero;

    private bool isAlive = true;
    // 起跳后上升中标示
    private bool isRising = false;
    // 下坠中标示
    private bool isFalling = false;
    private bool inGround = false;


    private SpriteRenderer sr;
    private Rigidbody2D rgBody2D;
    private Animator animator;

    private float jumpStartTime = 0;
    private float fallingStartTime = 0;

    private Transform groundTrans = null;
    private Vector3 groundPoint = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D raycast2D = Physics2D.Raycast(transform.position, Vector3.down, Tool.GetSpriteSizeInScene(sr).y * 0.5f + 0.1f, LayerMask.GetMask("Ground"));
        inGround = raycast2D.normal.y > 0;
        //RaycastHit2D raycast2D = Physics2D.Raycast(transform.position, Vector3.down, Tool.GetSpriteSizeInScene(sr).y * 2, LayerMask.GetMask("Ground"));
        //groundTrans = raycast2D.transform;
        //groundPoint = raycast2D.point;

        //if(groundTrans != null)
        //{
        //    if(groundTrans.position.y + Tool.GetSpriteSizeInScene(groundTrans.GetComponent<SpriteRenderer>()).y*0.5f+ Tool.GetSpriteSizeInScene(sr).y*0.5 >= transform.position.y)
        //    {
        //        inGround = true;
        //    }
        //    else
        //    {
        //        inGround = false;
        //    }
        //}

        float h = Input.GetAxis("Horizontal");
        if (h != 0)
        {
            float direction = h / Mathf.Abs(h);
            if (velocity.x == 0)
            {
                velocity.x = startHorizontalSpeed * direction;
            }
            else
            {
                if (velocity.x * h > 0)
                {
                    velocity.x += 0.15f * direction;
                }
                else
                {
                    velocity.x += 0.5f * direction;
                }
            }
            velocity.x = Mathf.Clamp(velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);
        }
        //else if (h == 0 && velocity.x != 0)
        //{
        //    float absVelocityX = Mathf.Abs(velocity.x);
        //    velocity.x = Mathf.Max(absVelocityX - 0.5f, 0) * velocity.x / absVelocityX;

        //}

        if (inGround)
        {
            velocity.y = 0;
            if (h > 0)
            {
                transform.rotation = Quaternion.identity;
            }
            else if (h < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (h == 0 && velocity.x != 0)
            {
                float absVelocityX = Mathf.Abs(velocity.x);
                velocity.x = Mathf.Max(absVelocityX - 0.5f, 0) * velocity.x / absVelocityX;
            }

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    jumpStartTime = Time.time;
            //    ChangeToRising();
            //}
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = startJumpSpeed;
            jumpStartTime = Time.time;
            ChangeToRising();
        }

        //if (isRising)
        if (!inGround)
            {
            //velocity.y -= gravity * (Time.time - jumpStartTime);
            velocity.y -= gravity * (Time.time - jumpStartTime);
            //if (Input.GetKeyUp(KeyCode.Space) || velocity.y <= 0)
            //{
            //    fallingStartTime = Time.time;
            //    ChangeToFalling();
            //}
            if (Input.GetKeyUp(KeyCode.Space))
            {
                velocity.y *= 0.5f; 
            }
        }
        else if (isFalling)
        {
            //velocity.y -= gravity * (Time.time - fallingStartTime);
            //velocity.y -= gravity * Time.deltaTime;
        }



        transform.position += Vector3.right * velocity.x * Time.deltaTime;
        //Vector3.MoveTowards(transform.position, groundPoint + Vector3.up * (Tool.GetSpriteSizeInScene(groundTrans.GetComponent<SpriteRenderer>()).y * 0.5f + Tool.GetSpriteSizeInScene(sr).y * 0.5f), velocity.y * Time.deltaTime);
        transform.position += Vector3.up * velocity.y * Time.deltaTime;
        //transform.position += (Vector3)velocity * Time.deltaTime;

        animator.SetFloat("speed_x", velocity.x);
        animator.SetFloat("force_x", h);
        if (animator.GetBool("inGround") != inGround)
        {
            animator.SetBool("inGround", inGround);
        }
    }

    void FixedUpdate()
    {
        RaycastHit2D raycast2D = Physics2D.Raycast(transform.position, Vector3.down, Tool.GetSpriteSizeInScene(sr).y * 0.5f + 0.1f, LayerMask.GetMask("Ground"));
        inGround = raycast2D.normal.y > 0;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    ContactPoint2D[] contacts = collision.contacts;
    //    for (int i = 0; i < contacts.Length; i++)
    //    {
    //        if (!inGround && contacts[i].normal.y < 0)
    //        {
    //            //ChangeToFalling();
    //            //rgBody2D.velocity = Vector2.zero;
    //            velocity.y = 0;
    //        }
    //        else if (!inGround && contacts[i].normal.y > 0)
    //        {
    //            inGround = true;
    //            velocity.y = 0;
    //        }
    //    }
    //}

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
