using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class BrokeBrick : MonoBehaviour
{
    public Vector2 direction = Vector2.up;
    public float initialSpeed = 20;

    private Rigidbody2D rgbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rgbody2D = GetComponent<Rigidbody2D>();
        rgbody2D.velocity = direction.normalized * initialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(Tool.IsOutOfCameraX(transform.position.x,1) || Tool.IsOutOfCameraY(transform.position.y, 1))
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, direction);
    }



    public static void SetBrokeBrickAttr(BrokeBrick brokeBrick, Vector2 direction, float initialSpeed)
    {
        if (brokeBrick != null)
        {
            brokeBrick.direction = direction;
            brokeBrick.initialSpeed = initialSpeed;
        }
    }
}
