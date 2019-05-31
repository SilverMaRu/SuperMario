using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputJump : Action
{
    //public MonoBehaviour owner;

    public KeyCode jumpKey;
    public float jumpForce;

    private bool onGround = false;
    private bool hadJumpKeyDown = false;
    private bool hadJumpKeyUp = false;
    private bool isAlive = true;

    private Rigidbody2D rgBody2D;
    private Animator animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        Assets.Scripts.Others.EventManager.BindingEvent("OnDie", Die);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Time.timeScale == 0 || !isAlive)
        {
            return;
        }
        // 通过垂直方向速度判断是否在地面
        onGround = rgBody2D.velocity.y == 0;
        Jump();

        // 再次判断是否在地面
        onGround = rgBody2D.velocity.y == 0;
        animator.SetBool("inGround", onGround);
    }

    private void Jump()
    {
        if (onGround && Input.GetKeyDown(jumpKey))
        {
            rgBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            hadJumpKeyDown = true;
            hadJumpKeyUp = false;
        }

        if (!onGround && hadJumpKeyDown && !hadJumpKeyUp && Input.GetKeyUp(jumpKey))
        {
            rgBody2D.velocity = Vector2.right * rgBody2D.velocity.x + Vector2.up * rgBody2D.velocity.y * 0.25f;
            hadJumpKeyUp = true;
            hadJumpKeyDown = false;
        }
    }

    private void Die()
    {
        isAlive = false;
    }
}
