using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCoin : MonoBehaviour
{
    public float jumpForce = 20f;
    public float liveTime = 1;

    private Rigidbody2D rgBody2D;

    void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        rgBody2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

        Destroy(gameObject, liveTime);
    }
}
