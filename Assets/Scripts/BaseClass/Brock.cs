using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class Brock : MonoBehaviour
{
    public enum ItemType
    {
        None = -1,
        Coin = 0,
        InvincibleStar = 1,
        LiveUpMushroom = 2,
        GroupUpItem = 99
    }
    public ItemType itemType = ItemType.None;
    public int itemNum = 1;
    protected int currentHitNum = 0;

    protected Animator animator;
    protected EndAnimation endAniamtion;

    protected int layer_Player;
    protected GameObject[] itemPrefabs;
    protected Mario.Status currentStatus;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitGameObjects();
        InitComponents();
        InitVariables();
    }

    protected virtual void InitGameObjects()
    {
        string itemsPath = "Prefabs/Items/";
        if (itemType != ItemType.GroupUpItem)
        {
            itemPrefabs = Resources.LoadAll<GameObject>(itemsPath);
        }
        else
        {
            itemPrefabs = Resources.LoadAll<GameObject>(itemsPath + "GroupUpItems");
        }
    }

    protected virtual void InitComponents()
    {
        animator = GetComponent<Animator>();
        endAniamtion = GetComponent<EndAnimation>();
    }

    protected virtual void InitVariables()
    {
        layer_Player = LayerMask.NameToLayer("Player");
    }

    protected virtual void OnHit()
    {
        switch (itemType)
        {
            case ItemType.None:
                endAniamtion.SetEndAction(() => { });
                break;
            case ItemType.Coin:
            case ItemType.LiveUpMushroom:
                endAniamtion.SetEndAction(() => { });
                SpawItem();
                break;
            case ItemType.GroupUpItem:
            case ItemType.InvincibleStar:
                endAniamtion.SetEndAction(SpawItem);
                break;
        }
        animator.SetTrigger("hit");
        currentHitNum++;
        animator.SetBool("empty", currentHitNum >= itemNum && itemType != ItemType.None);
    }

    protected virtual void SpawItem()
    {
        if (itemType != ItemType.GroupUpItem)
        {
            int index = Mathf.Min((int)itemType, itemPrefabs.Length - 1);
            Instantiate(itemPrefabs[index], transform.position - Vector3.up * 0.5f, Quaternion.identity);
        }
        else
        {
            int index = Mathf.Min((int)currentStatus, itemPrefabs.Length - 1);
            Instantiate(itemPrefabs[index], transform.position - Vector3.up * 0.5f, Quaternion.identity);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        int colliderLayer = collision.gameObject.layer;
        if (colliderLayer == layer_Player && (currentHitNum < itemNum || itemType == ItemType.None))
        {
            ContactPoint2D[] contacts = collision.contacts;
            for (int i = 0; i < contacts.Length; i++)
            {
                if (contacts[i].normal.y > 0)
                {
                    Mario mario = collision.gameObject.GetComponent<Mario>();
                    currentStatus = mario.status;
                    OnHit();
                    break;
                }
            }
        }
    }
}
