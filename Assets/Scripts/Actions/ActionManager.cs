using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager
{
    public static Component[] AddActioinsFromExternalText<T>(T owner, string txtPath) where T : MonoBehaviour
    {
        if (!System.IO.File.Exists(txtPath)) return null;
        string[] actionNames = System.IO.File.ReadAllLines(txtPath);
        return AddActioins(owner, actionNames);
    }

    public static Component[] AddActioins<T>(T owner, string[] actionNames) where T : MonoBehaviour
    {
        Type[] types = new Type[actionNames.Length];
        for(int i = 0; i < actionNames.Length; i++)
        {
            types[i] = Type.GetType(actionNames[i]);
        }
        return AddActioins(owner, types);
    }

    public static Component[] AddActioins<T>(T owner, Type[] actionTypes) where T : MonoBehaviour
    {
        Component[] retComponents = new Component[actionTypes.Length];
        for(int i = 0; i < actionTypes.Length; i++)
        {
            MonoBehaviour action = (MonoBehaviour)owner.gameObject.AddComponent(actionTypes[i]);
            retComponents[i] = action;
            Type[] types = { typeof(MonoBehaviour) };
            object[] paramArray = { owner };
            //retComponents[i].GetType().GetMethod("Init")?.Invoke(retComponents[i], paramArray);
            actionTypes[i].GetMethod("Init")?.MakeGenericMethod(owner.GetType())?.Invoke(action, paramArray);
            //retComponents[i].GetType().GetMethod("Init", types)?.Invoke(retComponents[i], paramArray);
            //Debug.Log(retComponents[i].GetType().GetMethod("Init"));
            //Debug.Log(retComponents[i].GetType().GetMethod("Init", types));
            //Debug.Log(retComponents[i].GetType().GetMethod("Init", System.Reflection.BindingFlags.Public));
        }
        return retComponents;
    }

    public static T AddActioin<T>(MonoBehaviour owner) where T : MonoBehaviour
    {
        T retComponent = null;
        retComponent = owner.gameObject.AddComponent<T>();
        return retComponent;
    }

}
