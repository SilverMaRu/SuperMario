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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int collideLayer = collision.gameObject.layer;
        if(collideLayer == layer_Player)
        {
            Destroy(gameObject);
            Mario mario = collision.GetComponent<Mario>();
            if(mario != null)
            {
                mario.StatusChangeTo(Mario.Status.FireBig);
            }
        }
    }
}
