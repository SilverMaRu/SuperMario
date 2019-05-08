﻿using System.Collections;
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

    public float startSpeedX = 1.5f;
    public float maxSpeedX = 7;
    public float maxSpeedUseTime = 1.5f;
    public float stopUseTime = 1f;
    public float jumpForce = 30;

    public RuntimeAnimatorController[] controllers;

    public float ghostTime = 2f;

    private Rigidbody2D rgBody2D;
    private Collider2D coll2D;
    private Animator animator;
    private MarioStatusChange statusChange;

    private float detalSpeedX = 0;
    private float stopTimeXMaxSpeed = 0;
    private float inputTime = 0;
    private bool onGround = false;
    private Vector2 velocity = Vector2.zero;
    private bool isAlive = true;
    // 无敌(吃了星星之后的无敌,自身带有攻击判断)
    private bool isInvincible = false;
    // 受伤后免疫伤害状态
    private bool isGhost = false;
    private int layer_Enemy;

    // Start is called before the first frame update
    void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        statusChange = GetComponent<MarioStatusChange>();

        detalSpeedX = maxSpeedX - startSpeedX;
        stopTimeXMaxSpeed = stopUseTime * maxSpeedX;

        layer_Enemy = LayerMask.NameToLayer("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0 || !isAlive)
        {
            return;
        }
        // 通过垂直方向速度判断是否在地面
        onGround = rgBody2D.velocity.y == 0;
        velocity = rgBody2D.velocity;
        // 获取当前朝向 (-1/1)
        //float currentDirection = (transform.rotation.eulerAngles.y - 90) / -90;
        float currentDirection = transform.right.x;
        float h = Input.GetAxis("Horizontal");
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
                //transform.rotation = Quaternion.Euler(0, 90 - 90 * inputDirection, 0);
                transform.right *= -1;
            }
            // 提速
            if (inputDirection * velocity.x >= 0)
            {
                velocity.x = (startSpeedX + detalSpeedX * (inputTime / maxSpeedUseTime)) * inputDirection;
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
        
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            rgBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        
        if (!onGround && Input.GetKeyUp(KeyCode.Space))
        {
            rgBody2D.velocity = Vector2.right * rgBody2D.velocity.x + Vector2.up * rgBody2D.velocity.y * 0.25f;
        }

        // 再次判断是否在地面
        onGround = rgBody2D.velocity.y == 0;
        animator.SetFloat("speed_x", rgBody2D.velocity.x);
        animator.SetFloat("force_x", h);
        animator.SetBool("inGround", onGround);

        TestInput();


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int colliderLayerMask = collision.gameObject.layer;
        ContactPoint2D[] contacts = collision.contacts;
        if(colliderLayerMask == layer_Enemy)
        {
            for(int i = 0; i < contacts.Length; i++)
            {
                Vector2 hitNormal = contacts[i].normal;
                if (hitNormal.x != 0 || hitNormal.y < 0)
                {
                    CollideWithEnemy();
                    break;
                }
            }
        }
    }

    private void CollideWithEnemy()
    {
        // 原始状态
        if(status == Status.NormalSmall)
        {
            Die();
        }
        else
        {
            OnGhost();
            StatusChangeTo(Status.NormalSmall);
            Invoke("OffGhost", ghostTime);
        }
    }

    private void Die()
    {
        isAlive = false;
        rgBody2D.velocity = Vector2.zero;
        animator.SetBool("isAlive", isAlive);
        Invoke("OnDieJump", 1f);
    }

    private void OnDieJump()
    {
        rgBody2D.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
        coll2D.enabled = false;
    }

    private void OnGhost()
    {
        isGhost = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, layer_Enemy, true);
    }

    private void OffGhost()
    {
        isGhost = false;
        Physics2D.IgnoreLayerCollision(gameObject.layer, layer_Enemy, false);
    }

    private void StatusChangeTo(Status newStatus)
    {
        statusChange.DoChange(newStatus);
        switch (newStatus)
        {
            case Status.NormalSmall:
                animator.runtimeAnimatorController = controllers[0];
                break;
            case Status.NormalBig:
                animator.runtimeAnimatorController = controllers[1];
                break;
            case Status.FireSmall:
                break;
            case Status.FireBig:
                animator.runtimeAnimatorController = controllers[2];
                break;
        }
        status = newStatus;
    }


    private void TestInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StatusChangeTo(Status.NormalSmall);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StatusChangeTo(Status.NormalBig);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StatusChangeTo(Status.FireBig);
        }
    }
}
