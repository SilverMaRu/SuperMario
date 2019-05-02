using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Brock
{
    public int maxHitNum = 1;
    private int currentHitNum = 0;

    protected override void OnHit()
    {
        currentHitNum++;
        animator.SetBool("empty", currentHitNum >= maxHitNum);
        base.OnHit();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Mario mario = collision.gameObject.GetComponent<Mario>();
        if (mario != null)
        {
            if (currentHitNum < maxHitNum)
            {
                base.OnCollisionEnter2D(collision);
            }
            else
            {

            }
        }
    }
}
