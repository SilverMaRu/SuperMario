using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject target;
    public float startMoveOffsetX = 4f;
    public float moveCenterUseTime = 2f;

    private Transform targetTrans;
    private Rigidbody2D targetRgBody2D;
    // 摄像机恒定的位置向量
    private Vector3 stableVector;
    private float moveCenterVelocityX = 0;

    private float totalTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        targetTrans = target.transform;
        targetRgBody2D = target.GetComponent<Rigidbody2D>();

        stableVector = Vector3.Scale(transform.position, Vector3.up + Vector3.forward);
        moveCenterVelocityX = startMoveOffsetX / moveCenterUseTime;
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistance = transform.position.x - targetTrans.position.x;
        float newPositionX = 0;
        float targetCurrentSpeedX = targetRgBody2D.velocity.x;
        if (currentDistance <= 0)
        {
            TotalTime(moveCenterUseTime);
            newPositionX = targetTrans.position.x;
            transform.position = Vector3.right * newPositionX + stableVector;
        }
        else if (currentDistance <= startMoveOffsetX && targetCurrentSpeedX > 0)
        {
            TotalTime(Time.deltaTime);
            float normalTotalTime = totalTime / moveCenterUseTime;
            newPositionX = targetTrans.position.x + startMoveOffsetX * (1 - normalTotalTime);
            transform.position = Vector3.right * newPositionX + stableVector;
        }
        else if (currentDistance <= startMoveOffsetX && targetCurrentSpeedX <= 0)
        {
            totalTime = (targetTrans.position.x + startMoveOffsetX - transform.position.x) / moveCenterVelocityX;
        }
        else
        {
            TotalTime(-moveCenterUseTime);
        }
    }

    private void TotalTime(float detalTime)
    {
        totalTime = Mathf.Clamp(totalTime + detalTime, 0, moveCenterUseTime);
    }
}
