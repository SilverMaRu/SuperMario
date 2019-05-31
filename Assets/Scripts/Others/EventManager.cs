using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Others
{
    public class EventManager
    {
        private static Dictionary<string, Delegate> delegateDic = new Dictionary<string, Delegate>();

        public static void DeclareEvent(string eventName)
        {
            if (!delegateDic.ContainsKey(eventName))
            {
                delegateDic.Add(eventName, null);
            }
        }

        private static void AddDelegate(string eventName, Delegate newAction)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue)
            {
                delegateDic[eventName] = Delegate.Combine(existingDelegate, newAction);
            }
            else
            {
                delegateDic.Add(eventName, newAction);
            }
        }

        public static void BindingEvent(string eventName, System.Action newAction)
        {
            AddDelegate(eventName, newAction);
        }

        public static void BindingEvent<T>(string eventName, Action<T> newAction)
        {
            AddDelegate(eventName, newAction);
        }

        public static void BindingEvent<T1, T2>(string eventName, Action<T1, T2> newAction)
        {
            AddDelegate(eventName, newAction);
        }

        public static void BindingEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> newAction)
        {
            AddDelegate(eventName, newAction);
        }

        public static void BindingEvent<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> newAction)
        {
            AddDelegate(eventName, newAction);
        }

        public static void OnEvent(string eventName)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue && existingDelegate != null)
            {
                existingDelegate.DynamicInvoke();
            }
        }

        public static void OnEvent<T>(string eventName, T arg)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue && existingDelegate != null)
            {
                existingDelegate.DynamicInvoke(arg);
            }
        }

        public static void OnEvent<T1, T2>(string eventName, T1 arg0, T2 arg1)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue && existingDelegate != null)
            {
                existingDelegate.DynamicInvoke(arg0, arg1);
            }
        }

        public static void OnEvent<T1, T2, T3>(string eventName, T1 arg0, T2 arg1, T3 arg2)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue && existingDelegate != null)
            {
                existingDelegate.DynamicInvoke(arg0, arg1, arg2);
            }
        }

        public static void OnEvent<T1, T2, T3, T4>(string eventName, T1 arg0, T2 arg1, T3 arg2, T4 arg3)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue && existingDelegate != null)
            {
                existingDelegate.DynamicInvoke(arg0, arg1, arg2, arg3);
            }
        }

        public static void RemoveEvent(string eventName, Delegate remaveAction)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue)
            {
                Delegate.Remove(existingDelegate, remaveAction);
            }
        }
    }
}