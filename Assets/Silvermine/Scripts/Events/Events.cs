using System;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class Events
{
    private static Events _instance = new Events();
    public static Events Instance
    {
        get { return _instance; }
    }

    Dictionary<Type, Delegate> _delegates;

    private Events()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
        _delegates = new Dictionary<Type, Delegate>();
    }

    //public delegate void EventDelegate<T>(T e) where T : GameEvent;

    public void AddListener<T>(Action<T> listener) where T : GameEvent
    {
        Type type = typeof(T);

        Delegate d;
        if (_delegates.TryGetValue(type, out d))
        {
            d = Delegate.Remove(d, listener);
            _delegates[type] = Delegate.Combine(d, listener);
        }
        else
        {
            _delegates[type] = listener;
        }
    }

    public void RemoveListener<T>(Action<T> listener) where T : GameEvent
    {
        Type type = typeof(T);

        Delegate d;
        if (_delegates.TryGetValue(type, out d))
        {
            Delegate existingDel = Delegate.Remove(d, listener);

            if (existingDel == null)
            {
                _delegates.Remove(type);
            }
            else
            {
                _delegates[type] = existingDel;
            }
        }
    }

    public void Raise<T>(T e) where T : GameEvent
    {
        if (e == null)
        {
            throw new ArgumentNullException("e");
        }

        Delegate d;
        if (_delegates.TryGetValue(typeof(T), out d))
        {
            if(d is Action<T> callback)
            {
                callback(e);
            }
        }
    }

    public void AddOneTimeListener<T>(Action<T> listener) where T : GameEvent
    {
        listener += (ev) =>
        {
            RemoveListener<T>(listener);
        };

        AddListener<T>(listener);
    }
}

public class GameEvent { };

public class OnlineEvent : GameEvent
{
    public enum EventType { REQUEST_SERVER_JOIN, JOINED_SERVER, SERVER_FAIL, REQUEST_FIND_OPPONENT, FOUND_OPPONENT};

    public EventType Type;

    public OnlineEvent(EventType type)
    {
        Type = type;
    }
}
