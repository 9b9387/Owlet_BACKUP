// EventCenter.cs
// Author: shihongyang shihongyang@weile.com
// Data: 11/1/2022
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Owlet
{
    public static class EventCenter
    {
        private static readonly Dictionary<int, UnityEvent<object>> events = new();

        public static void Subscribe(int eventID, UnityAction<object> action)
        {
            if(events.ContainsKey(eventID) == false)
            {
                var e = new UnityEvent<object>();
                events.Add(eventID, e);
            }

            events[eventID].AddListener(action);
        }

        public static void Unsubscribe(int eventID, UnityAction<object> action)
        {
            if(events.ContainsKey(eventID) == false)
            {
                return;
            }

            events[eventID].RemoveListener(action);
        }

        public static void UnsubscribeAll()
        {
            foreach (var e in events.Values)
            {
                e.RemoveAllListeners();
            }

            events.Clear();
        }

        public static void Trigger(int eventID, object param = null)
        {
            if (events.ContainsKey(eventID) == false)
            {
                Debug.LogWarning($"No event subscribed eventID:{eventID}");
                return;
            }

            events[eventID].Invoke(param);
        }
    }
}