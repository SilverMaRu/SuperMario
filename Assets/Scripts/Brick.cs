using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Brock
{
    public string brokeBrickPrefabPath = "Prefabs/BrokeBrick";
    private GameObject brokeBrickPrefab;

    protected override void InitGameObjects()
    {
        base.InitGameObjects();
        brokeBrickPrefab = Resources.Load<GameObject>(brokeBrickPrefabPath);
    }

    private void Break()
    {
        if(brokeBrickPrefab != null)
        {
            InstantiateBrokeBrick();
        }
        Destroy(gameObject);
    }

    private void InstantiateBrokeBrick()
    {
        GameObject instance_0 = Instantiate(brokeBrickPrefab, transform.position - Vector3.right * 0.25f + Vector3.up * 0.25f, Quaternion.identity);
        BrokeBrick.SetBrokeBrickAttr(instance_0.GetComponent<BrokeBrick>(), Vector2.left + Vector2.up * 3, 20);

        GameObject instance_1 = Instantiate(brokeBrickPrefab, transform.position + Vector3.right * 0.25f + Vector3.up * 0.25f, Quaternion.Euler(0,0,90));
        BrokeBrick.SetBrokeBrickAttr(instance_1.GetComponent<BrokeBrick>(), Vector2.right + Vector2.up * 3, 20);

        GameObject instance_2 = Instantiate(brokeBrickPrefab, transform.position - Vector3.right * 0.25f - Vector3.up * 0.25f, Quaternion.identity);
        BrokeBrick.SetBrokeBrickAttr(instance_2.GetComponent<BrokeBrick>(), Vector2.left + Vector2.up * 3, 10);

        GameObject instance_3 = Instantiate(brokeBrickPrefab, transform.position + Vector3.right * 0.25f - Vector3.up * 0.25f, Quaternion.Euler(0, 0, 90));
        BrokeBrick.SetBrokeBrickAttr(instance_3.GetComponent<BrokeBrick>(), Vector2.right + Vector2.up * 3, 10);
    }

    protected override void OnHit()
    {
        base.OnHit();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Mario mario = collision.gameObject.GetComponent<Mario>();
        if(mario != null)
        {
            if((int)mario.status < 10)
            {
                base.OnCollisionEnter2D(collision);
            }
            else
            {
                Break();
            }
        }
    }
}
