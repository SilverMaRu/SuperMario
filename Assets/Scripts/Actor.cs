using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour, IActivite
{
    public Dictionary<Type, Activity> activites { get { return _activites; } }
    protected Dictionary<Type, Activity> _activites;
}
