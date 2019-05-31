using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Item
{
    private int layer_Player;

    protected override void Start()
    {
        base.Start();
        layer_Player = LayerMask.NameToLayer("Player");
        _score = 1000;
    }

    protected override void EndRise()
    {
        if (totalTime > spawUseTime)
        {
            coll2D.enabled = true;
        }
    }
}
