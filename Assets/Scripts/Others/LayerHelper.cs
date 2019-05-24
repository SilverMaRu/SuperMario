using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHelper
{
    private static Dictionary<string, int> layers = new Dictionary<string, int>();

    public static int GetLayer(string name)
    {
        int retLayer = -1;
        if(!layers.TryGetValue(name, out retLayer))
        {
            retLayer = LayerMask.NameToLayer(name);
            layers.Add(name, retLayer);
        }
        return retLayer;
    }

    public static void OffLayerCollisionMask(string layerName1, string layerName2)
    {
        OffLayerCollisionMask(GetLayer(layerName1), GetLayer(layerName2));
    }

    public static void OffLayerCollisionMask(int layer1, int layer2)
    {
        Physics2D.SetLayerCollisionMask(layer1, 0 << layer2);
    }

    public static void OnLayerCollisionMask(string layerName1, string layerName2)
    {
        OnLayerCollisionMask(GetLayer(layerName1), GetLayer(layerName2));
    }
    public static void OnLayerCollisionMask(int layer1, int layer2)
    {
        Physics2D.SetLayerCollisionMask(layer1, 1 << layer2);
    }
}
