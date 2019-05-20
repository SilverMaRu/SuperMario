using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalActingPlay : MonoBehaviour
{
    public float speedDown = 10f;
    public float speedX = 7f;

    private Rigidbody2D rgBody2D;
    //private float gravityScaleMark = 0;
    private Vector3 bottomPosition;

    private bool isStartDown = false;
    private bool isMoveToBottom = false;
    private bool isTurned = false;
    private bool isEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if(rgBody2D == null)
        {
            rgBody2D = GetComponent<Rigidbody2D>();
        }
        rgBody2D.isKinematic = true;
        bottomPosition = Vector3.Scale(transform.localPosition, Vector3.right + Vector3.forward);
        isStartDown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnd)
        {
            rgBody2D.velocity = transform.right * speedX;
        }
        if (isTurned && !isEnd)
        {
            transform.right *= -1;
            rgBody2D.isKinematic = false;
            isEnd = true;
        }
        if (isMoveToBottom && !isTurned)
        {
            transform.parent = null;
            transform.position = transform.position + Vector3.right;
            transform.right *= -1;
            isTurned = true;
        }
        if (!isMoveToBottom && isStartDown)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, bottomPosition, speedDown * Time.deltaTime);
            isMoveToBottom = transform.localPosition == bottomPosition;
        }
    }
}
