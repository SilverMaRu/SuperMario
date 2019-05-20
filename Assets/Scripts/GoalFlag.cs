using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalFlag : MonoBehaviour
{
    public Transform flagTrans;
    public float speedDown = 10f;

    private bool shouldDown = false;
    private Vector3 bottomPosition;

    // Start is called before the first frame update
    void Start()
    {
        bottomPosition = Vector3.Scale(flagTrans.localPosition, Vector3.right + Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldDown && flagTrans.localPosition != bottomPosition)
        {
            flagTrans.localPosition = Vector3.MoveTowards(flagTrans.localPosition, bottomPosition, speedDown * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerHelper.GetLayer("Player"))
        {
            shouldDown = true;
        }
    }
}
