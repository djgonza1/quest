using System;
using System.Collections.Generic;

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

    public delegate void EventDelegate<T>(T e) where T : GameEvent;

    public void AddListener<T>(EventDelegate<T> listener) where T : GameEvent
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

    public void RemoveListener<T>(EventDelegate<T> listener) where T : GameEvent
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
            if(d is EventDelegate<T> callback)
            {
                callback(e);
            }
        }
    }
}

public class GameEvent { };

public class CardEvent : GameEvent
{
    public enum EventType { ENTER, EXIT, HOVER, TAP_DOWN, TAP_RELEASE, DRAG };

    public EventType Type;
    public CardObject Card;

    public CardEvent(EventType type, CardObject card)
    {
        this.Type = type;
        this.Card = card;
    }
}