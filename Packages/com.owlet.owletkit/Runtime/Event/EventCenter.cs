// EventCenter.cs
// Author: shihongyang shihongyang@weile.com
// Data: 11/1/2022
using System.Collections.Generic;
using UnityEngine.Events;

namespace Owlet
{
    public static class EventCenter
    {
        private static readonly Dictionary<string, UnityEvent<object>> events = new Dictionary<string, UnityEvent<object>>();

        public static void Subscribe(string eventName, UnityAction<object> action)
        {
            if(events.ContainsKey(eventName) == false)
            {
                var e = new UnityEvent<object>();
                events.Add(eventName, e);
            }

            events[eventName].AddListener(action);
        }

        public static void Unsubscribe(string eventName, UnityAction<object> action)
        {
            if(events.ContainsKey(eventName) == false)
            {
                return;
            }

            events[eventName].RemoveListener(action);
        }

        public static void UnsubscribeAll()
        {
            foreach (var e in events.Values)
            {
                e.RemoveAllListeners();
            }

            events.Clear();
        }

        public static void Trigger(string eventName, object param)
        {
            if (events.ContainsKey(eventName) == false)
            {
                var e = new UnityEvent<object>();
                events.Add(eventName, e);
            }

            events[eventName].Invoke(param);
        }
    }
}