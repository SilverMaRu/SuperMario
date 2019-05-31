using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager
{
    public static Component[] AddActionsFromExternalText<T>(T owner, string txtPath) where T : MonoBehaviour
    {
        if (!System.IO.File.Exists(txtPath)) return null;
        string[] actionNames = System.IO.File.ReadAllLines(txtPath);
        return AddActions(owner, actionNames);
    }

    public static Component[] AddActions<T>(T owner, string[] actionNames) where T : MonoBehaviour
    {
        Type[] types = new Type[actionNames.Length];
        for(int i = 0; i < actionNames.Length; i++)
        {
            types[i] = Type.GetType(actionNames[i]);
        }
        return AddActions(owner, types);
    }

    public static Component[] AddActions<T>(T owner, Type[] actionTypes) where T : MonoBehaviour
    {
        Component[] retComponents = new Component[actionTypes.Length];
        for(int i = 0; i < actionTypes.Length; i++)
        {
            Action action = (Action)owner.gameObject.AddComponent(actionTypes[i]);
            retComponents[i] = action;
            action.Init(owner);
        }
        return retComponents;
    }

    public static T AddAction<T>(MonoBehaviour owner) where T : MonoBehaviour
    {
        T retComponent = null;
        retComponent = owner.gameObject.AddComponent<T>();
        return retComponent;
    }

}
