using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject target;
    public float startMoveOffsetX = 4f;
    public float moveCenterUseTime = 2f;

    private Transform targetTrans;
    // 摄像机恒定的位置向量
    private Vector3 stableVector;
    private float moveCenterVelocityX = 0;
    private float lastPositionX = 0;

    private float totalTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        targetTrans = target.transform;

        stableVector = Vector3.Scale(transform.position, Vector3.up + Vector3.forward);
        moveCenterVelocityX = startMoveOffsetX / moveCenterUseTime;
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistance = transform.position.x - targetTrans.position.x;
        float newPositionX = 0;
        float targetDeltaPositionX = targetTrans.position.x - lastPositionX;
        if (currentDistance <= 0)
        {
            TotalTime(moveCenterUseTime);
            newPositionX = targetTrans.position.x;
            transform.position = Vector3.right * newPositionX + stableVector;
        }
        else if (currentDistance <= startMoveOffsetX && targetDeltaPositionX > 0)
        {
            float normalTotalTime = Mathf.Min(totalTime / moveCenterUseTime, 1);
            newPositionX = targetTrans.position.x + startMoveOffsetX * (1 - normalTotalTime);
            if(newPositionX> transform.position.x)
            {
                transform.position = Vector3.right * newPositionX + stableVector;
                TotalTime(Time.deltaTime);
            }
        }
        else if (currentDistance <= startMoveOffsetX && targetDeltaPositionX == 0) { }
        else if (currentDistance <= startMoveOffsetX && targetDeltaPositionX < 0)
        {
            totalTime = (targetTrans.position.x + startMoveOffsetX - transform.position.x) / moveCenterVelocityX;
        }
        else
        {
            TotalTime(-moveCenterUseTime);
        }
        lastPositionX = targetTrans.position.x;
    }

    private void TotalTime(float detalTime)
    {
        totalTime = Mathf.Clamp(totalTime + detalTime, 0, moveCenterUseTime);
    }
}
