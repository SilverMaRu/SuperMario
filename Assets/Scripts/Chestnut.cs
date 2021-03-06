﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class Chestnut : Enemy
{
    protected override void Start()
    {
        base.Start();
        _score = 100;
    }

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
        rgBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        Die();
    }
}
