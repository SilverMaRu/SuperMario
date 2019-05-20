using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityMove : Activity
{
    //public float startSpeedX = 1.5f;
    //public float maxSpeedX = 7;
    //public float maxSpeedUseTime = 1.5f;
    //public float stopUseTime = 1f;
    public float startSpeedX { get { return startSpeedX; } set { startSpeedX = value; deltaSpeedX = maxSpeedX - startSpeedX; } }
    public float maxSpeedX { get { return maxSpeedX; } set { maxSpeedX = value; deltaSpeedX = maxSpeedX - startSpeedX; } }
    public float maxSpeedUseTime { get; set; }
    public float stopUseTime { get; set; }

    private Vector3 velocity;

    private float deltaSpeedX = 0;
    //private float stopTimeXMaxSpeed = 0;
    private float inputTime = 0;
    private bool onGround = false;

    public ActivityMove(Actor owner) : base(owner)
    {
    }

    public ActivityMove(Actor owner, float startSpeedX, float maxSpeedX, float maxSpeedUseTime, float stopUseTime) : this(owner)
    {
        this.startSpeedX = startSpeedX;
        this.maxSpeedX = maxSpeedX;
        this.maxSpeedUseTime = maxSpeedUseTime;
        this.stopUseTime = stopUseTime;
    }

    public override void Update()
    {
        //float h = Input.GetAxis("Horizontal");
        Activity activity = null;
        if (owner.activites.TryGetValue(typeof(ActivityInput), out activity))
        {
            Move(((ActivityInput)activity).h);
        }
    }

    private void Move(float h)
    {
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
        rigidbody2D.velocity = velocity;
    }
}
