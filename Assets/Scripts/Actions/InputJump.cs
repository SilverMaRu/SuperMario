using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputJump : MonoBehaviour
{
    //public MonoBehaviour owner;

    public KeyCode jumpKey { get; set; }
    public float jumpForce { get; set; }

    private bool onGround = false;
    private bool hadJumpKeyDown = false;
    private bool hadJumpKeyUp = false;
    private bool isAlive = true;

    private Rigidbody2D rgBody2D;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Init();
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

    //public void Init(KeyCode jumpKey, float jumpForce)
    public void Init<T>(T owner) where T : MonoBehaviour
    {
        //System.Reflection.FieldInfo[] publicFields = typeof(T).GetFields(System.Reflection.BindingFlags.Public);
        //System.Reflection.PropertyInfo[] publicPropertys = GetType().GetProperties(System.Reflection.BindingFlags.Public);
        System.Reflection.FieldInfo[] publicFields = typeof(T).GetFields();
        System.Reflection.PropertyInfo[] publicPropertys = GetType().GetProperties();
        foreach (System.Reflection.FieldInfo field in publicFields)
        {
            foreach (System.Reflection.PropertyInfo property in publicPropertys)
            {
                if (field.Name == property.Name)
                {
                    property.SetValue(this, field.GetValue(owner));
                }
            }
        }

        Init();
    }

    private void Init()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        Assets.Scripts.Others.EventManager.BindingEvent("OnDie", Die);
    }
}
