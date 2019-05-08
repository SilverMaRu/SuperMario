using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class Chestnut : Enemy
{
    protected override void Update()
    {
        if(Tool.IsOutOfCameraY(transform.position.y, 1.5f))
        {
            Destroy(gameObject);
        }
    }

    protected override void StepedOn()
    {
        base.StepedOn();
        Die();
    }
}
