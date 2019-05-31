using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public void Init<T>(T owner) where T : MonoBehaviour
    {
        System.Reflection.FieldInfo[] publicFields = owner.GetType().GetFields();
        foreach (System.Reflection.FieldInfo field in publicFields)
        {
            System.Reflection.FieldInfo tempField = GetType().GetField(field.Name);
            if (tempField != null)
            {
                tempField.SetValue(this, field.GetValue(owner));
            }
        }
    }
}
