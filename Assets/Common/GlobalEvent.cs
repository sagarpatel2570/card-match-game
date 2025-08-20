using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [DefaultExecutionOrder(-100000)]
    public static class GlobalEvents
    {
        private static readonly Dictionary<Type, Delegate> EventTable = new Dictionary<Type, Delegate>();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnAppStart()
        {
            Debug.Log("Application started (Before first scene loads).");
            EventTable.Clear();
        }

        public static void Register<T>(Action<T> listener)
        {
            var type = typeof(T);
            if (EventTable.TryGetValue(type, out var existing))
            {
                EventTable[type] = Delegate.Combine(existing, listener);
            }
            else
            {
                EventTable[type] = listener;
            }
        }

        public static void UnRegister<T>(Action<T> listener)
        {
            var type = typeof(T);
            if (EventTable.TryGetValue(type, out var existing))
            {
                var newDelegate = Delegate.Remove(existing, listener);
                if (newDelegate == null)
                    EventTable.Remove(type);
                else
                    EventTable[type] = newDelegate;
            }
        }

        public static void Trigger<T>(T eventData)
        {
            var type = typeof(T);
            if (EventTable.TryGetValue(type, out var del))
            {
                if (del is Action<T> callback)
                {
                    try
                    {
                        callback.Invoke(eventData);
                    }
                    catch (Exception e)
                    {
                       Debug.LogError($"Callback failed for type {type} with exception {e} ");
                        throw;
                    }
                }
            }
        }
    }
}
