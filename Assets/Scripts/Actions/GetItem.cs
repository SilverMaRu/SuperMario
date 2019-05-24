using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class GetItem : MonoBehaviour
{
    private int layer_Item;

    // Start is called before the first frame update
    void Start()
    {
        layer_Item = LayerHelper.GetLayer("Item");
        //LayerHelper.OffLayerCollisionMask(gameObject.layer, layer_Item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.tag == "Item")
    //    {
    //        Destroy(collision.gameObject);
    //        Item item = collision.GetComponent<Item>();

    //        if(item != null)
    //        {
    //            string itemClassName = item.GetType().Name;
    //            EventManager.OnEvent("GetItem", item.GetType());
    //        }
    //    }
    //}
}
