using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class BulletFire : MonoBehaviour
{
    public float spawSpeed = 10f;
    public float springSpeed = 10f;
    public Vector3 springAngle = Vector3.forward * 45;

    private Rigidbody2D rgBody2D;
    private Collider2D coll2D;

    private int layer_Ground;
    private int layer_Enemy;
    private Quaternion angle;
    private float gravityScaleMark = 0;
    private GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        gravityScaleMark = rgBody2D.gravityScale;
        rgBody2D.gravityScale = 0;
        rgBody2D.velocity = transform.right * spawSpeed;
        coll2D = GetComponent<Collider2D>();

        layer_Ground = LayerMask.NameToLayer("Ground");
        layer_Enemy = LayerMask.NameToLayer("Enemy");
        explosion = Resources.Load<GameObject>("Prefabs/Explosion");
        angle = Quaternion.Euler(Vector3.up * transform.rotation.eulerAngles.y + springAngle);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        if (Tool.IsOutOfCameraX(position.x, 0.5f) || Tool.IsOutOfCameraY(position.y, 0.5f))
        {
            DestroyFire();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int collideLayer = collision.gameObject.layer;
        if(collideLayer == layer_Ground)
        {
            ContactPoint2D[] contacts = collision.contacts;
            for(int i = 0; i < contacts.Length; i++)
            {
                if(contacts[i].normal.y > 0)
                {
                    Spring();
                    break;
                }
                if (contacts[i].normal.x != 0)
                {
                    HitGround();
                    break;
                }
            }
        }
        else if(collideLayer == layer_Enemy)
        {
            DestroyFire();
        }
    }

    private void Spring()
    {
        transform.rotation = angle;
        rgBody2D.gravityScale = gravityScaleMark;
        rgBody2D.velocity = transform.right * springSpeed;
    }

    private void HitGround()
    {
        DestroyFire();
    }

    public void DestroyFire()
    {
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        Mario.bulletNum = Mathf.Max(Mario.bulletNum - 1, 0);
    }
}
