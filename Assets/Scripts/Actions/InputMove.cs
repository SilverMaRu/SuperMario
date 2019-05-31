using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMove : Action
{
    public float startSpeedX;
    public float maxSpeedX;
    public float maxSpeedUseTime;
    public float stopUseTime;

    private float deltaSpeedX = 0;
    private float stopTimeXMaxSpeed = 0;
    private float inputTime = 0;
    private Vector2 velocity = Vector2.zero;
    private bool onGround = false;
    private bool isAlive = true;

    private Rigidbody2D rgBody2D;
    private Animator animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        deltaSpeedX = maxSpeedX - startSpeedX;
        stopTimeXMaxSpeed = stopUseTime * maxSpeedX;

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
        float h = Input.GetAxis("Horizontal");
        Move(h);

        // 再次判断是否在地面
        onGround = rgBody2D.velocity.y == 0;
        animator.SetFloat("speed_x", rgBody2D.velocity.x);
        animator.SetFloat("force_x", h);
        animator.SetBool("inGround", onGround);
    }

    private void Move(float h)
    {
        velocity = rgBody2D.velocity;
        // 获取当前朝向 (-1/1)
        float currentDirection = transform.right.x;
        // 有水平输入
        if (h != 0)
        {
            // 获取水平输入方向 (-1/1)
            float inputDirection = h / Mathf.Abs(h);
            bool isSameDirection = inputDirection == currentDirection;
            // 转向/重新起步时重置inputTime
            if (!isSameDirection || velocity.x == 0)
            {
                inputTime = 0;
            }
            // 累计inputTime 刹车时最大值为stopUseTime,提速时最大值为maxSpeedUseTime
            inputTime = Mathf.Min(inputTime + Time.deltaTime, inputDirection * velocity.x < 0 ? stopUseTime : maxSpeedUseTime);

            // 在地面
            if (onGround && !isSameDirection)
            {
                // 改变朝向
                transform.right *= -1;
            }
            // 提速
            if (inputDirection * velocity.x >= 0)
            {
                velocity.x = (startSpeedX + deltaSpeedX * (inputTime / maxSpeedUseTime)) * inputDirection;
            }
            else // 刹车
            {
                velocity.x = velocity.x * (1 - inputTime / stopUseTime);
            }
        }
        else if (onGround && h == 0) // 自然停下
        {
            velocity.x = velocity.x - maxSpeedX / stopUseTime * Time.deltaTime * currentDirection;
            if (velocity.x * currentDirection < 0)
            {
                velocity.x = 0;
            }
        }
        rgBody2D.velocity = velocity;
    }

    private void Die()
    {
        isAlive = false;
    }
}
