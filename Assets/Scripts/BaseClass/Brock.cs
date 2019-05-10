using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brock : MonoBehaviour
{
    public enum ItemType
    {
        None,
        Coin,
        GroupUpItem,
        InvincibleStar,
        LiveupMushroom
    }
    public ItemType itemType = ItemType.None;
    public int itemNum = 1;
    protected int currentHitNum = 0;

    protected Animator animator;

    protected int layer_Player;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitGameObjects();
        InitComponents();
        InitVariables();
    }

    protected virtual void InitGameObjects() { }

    protected virtual void InitComponents()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void InitVariables()
    {
        layer_Player = LayerMask.NameToLayer("Player");
    }

    protected virtual void OnHit()
    {
        animator.SetTrigger("hit");
        currentHitNum++;
        animator.SetBool("empty", currentHitNum >= itemNum && itemType != ItemType.None);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Mario mario = collision.gameObject.GetComponent<Mario>();
        int colliderLayer = collision.gameObject.layer;
        if (colliderLayer == layer_Player && (currentHitNum < itemNum || itemType == ItemType.None))
        {
            ContactPoint2D[] contacts = collision.contacts;
            for (int i = 0; i < contacts.Length; i++)
            {
                if (contacts[i].normal.y > 0)
                {
                    OnHit();
                    break;
                }
            }
        }
    }
}
