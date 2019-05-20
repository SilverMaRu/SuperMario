using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHelper
{
    private static Dictionary<string, int> layers = new Dictionary<string, int>();

    private LayerHelper() { }

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
}
