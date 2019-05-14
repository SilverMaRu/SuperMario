using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // 生成过程所用时间
    public float spawUseTime = 1f;
    // 生成后上升的距离
    public float spawRiseDistance = 1f;
    public float speedX = 5f;

    protected Rigidbody2D rgBody2D;
    protected Collider2D coll2D;

    // 上升过程的速度
    protected float speedRise;

    protected float totalTime = 0f;
    // 标志是否已经调用过Push方法
    protected bool done = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<Collider2D>();
        rgBody2D.isKinematic = true;
        coll2D.enabled = false;

        speedRise = spawRiseDistance / spawUseTime;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Rise();
        EndRise();
    }

    // 生成道具后升起
    protected virtual void Rise()
    {
        if (totalTime <= spawUseTime)
        {
            transform.position += Vector3.up * speedRise * Time.deltaTime;
            totalTime += Time.deltaTime;
        }
    }

    // 上升结束后使道具移动
    protected virtual void EndRise()
    {
        if (totalTime > spawUseTime && !done)
        {
            rgBody2D.isKinematic = false;
            coll2D.enabled = true;
            rgBody2D.velocity = transform.right * speedX;
            done = true;
        }
    }
}
