using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;
public class Mario : MonoBehaviour
{
    public enum Status
    {
        NormalSmall = 0,
        NormalBig = 1,
        FireBig = 2
    }
    public static int bulletNum = 0;

    public KeyCode jumpKey = KeyCode.K;
    public KeyCode shootKey = KeyCode.J;

    public Status status = Status.NormalSmall;

    public float startSpeedX = 1.5f;
    public float maxSpeedX = 7;
    public float maxSpeedUseTime = 1.5f;
    public float stopUseTime = 1f;
    public float jumpForce = 30;

    public RuntimeAnimatorController[] controllers;

    public float ghostTime = 2f;

    public Transform shootPointTrans;
    public float shootFrequency = 0.5f;
    public int maxBulletAliveNum = 2;

    private Rigidbody2D rgBody2D;
    private Collider2D coll2D;
    private Animator animator;
    private MarioStatusChange statusChange;

    private float deltaSpeedX = 0;
    private float stopTimeXMaxSpeed = 0;
    private float inputTime = 0;
    private bool onGround = false;
    // 记录是否按下过跳跃按键
    private bool hadJumpKeyDown = false;
    // 记录是否释放过跳跃按键
    private bool hadJumpKeyUp = false;
    private Vector2 velocity = Vector2.zero;
    private bool isAlive = true;
    // 无敌(吃了星星之后的无敌,自身带有攻击判断)
    private bool isInvincible = false;
    // 受伤后免疫伤害状态
    private bool isGhost = false;
    private float lastShootTime = 0;

    //private int layer_Enemy;
    private GameObject bulletFirePrefab;

    // Start is called before the first frame update
    void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        statusChange = GetComponent<MarioStatusChange>();

        deltaSpeedX = maxSpeedX - startSpeedX;
        stopTimeXMaxSpeed = stopUseTime * maxSpeedX;

        //layer_Enemy = LayerMask.NameToLayer("Enemy");
        bulletFirePrefab = Resources.Load<GameObject>("Prefabs/Fire");

        EventManager.BindingEvent<IScore>("AddScore", OnAddScore);
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
        float h = Input.GetAxis("Horizontal");
        Move(h);
        Jump();
        Shoot();

        // 再次判断是否在地面
        onGround = rgBody2D.velocity.y == 0;
        animator.SetFloat("speed_x", rgBody2D.velocity.x);
        animator.SetFloat("force_x", h);
        animator.SetBool("inGround", onGround);
        animator.SetBool("isGhost", isGhost);

        TestInput();


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
        rgBody2D.velocity = velocity;
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

    private void Shoot()
    {
        if (status != Status.FireBig)
        {
            return;
        }
        if (Input.GetKeyDown(shootKey) && bulletNum < maxBulletAliveNum && Time.time - lastShootTime> shootFrequency)
        {
            animator.SetTrigger("shoot");
            if(bulletFirePrefab != null && shootPointTrans != null)
            {
                Instantiate(bulletFirePrefab, shootPointTrans.position, shootPointTrans.rotation);
                bulletNum++;
            }
            lastShootTime = Time.time;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int colliderLayer = collision.gameObject.layer;
        ContactPoint2D[] contacts = collision.contacts;
        //if(colliderLayerMask == layer_Enemy)
        if (colliderLayer == LayerHelper.GetLayer("Enemy"))
        {
            for (int i = 0; i < contacts.Length; i++)
            {
                Vector2 hitNormal = contacts[i].normal;
                if (hitNormal.x != 0 || hitNormal.y < 0)
                {
                    CollideWithEnemy();
                    break;
                }
            }
        }
        else if(colliderLayer == LayerHelper.GetLayer("GoalFlag"))
        {
            Goal();
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
        //Physics2D.IgnoreLayerCollision(gameObject.layer, layer_Enemy, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerHelper.GetLayer("Enemy"), true);
    }

    private void OffGhost()
    {
        isGhost = false;
        //Physics2D.IgnoreLayerCollision(gameObject.layer, layer_Enemy, false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerHelper.GetLayer("Enemy"), false);
    }

    private void Goal()
    {
        GameObject goalFlagGameObj = GameObject.FindGameObjectWithTag("GoalFlag");
        Vector3 goalFlagPosition = goalFlagGameObj.transform.position;
        Vector3 myPosition = transform.position;
        if (goalFlagPosition.y < myPosition.y)
        {
            myPosition.y = goalFlagPosition.y;
            transform.position = myPosition;
        }
        transform.parent = goalFlagGameObj.transform.parent;
        GetComponent<GoalActingPlay>().enabled = true;
    }

    public void StatusChangeTo(Status newStatus)
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
            case Status.FireBig:
                animator.runtimeAnimatorController = controllers[2];
                break;
        }
        status = newStatus;
    }

    public void OnInvincible()
    {
        isInvincible = true;
    }

    private void OffInvincible()
    {
        isInvincible = false;
    }

    private void OnAddScore(IScore score)
    {
        OnAddScore(score.score);
    }

    private void OnAddScore(int score)
    {
        Debug.LogFormat("Mario get {0} point", score);
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
