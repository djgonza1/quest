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

public class CardObjectEvent : GameEvent
{
    public enum EventType { ENTER, EXIT, HOVER, TAP_DOWN, TAP_RELEASE, DRAG, PLAYED };

    public EventType Type;
    public CardGO CardObject;
    public Player Player;

    public CardObjectEvent(EventType type, CardGO card, Player player = Player.None)
    {
        this.Type = type;
        this.CardObject = card;
        this.Player = player;
    }
}

public class SessionCardEvent : GameEvent
{
    public enum EventType { PLAYED };

    public EventType Type; 
    public AbilityCard Card;
    public Player Player;

    public SessionCardEvent(EventType type, AbilityCard card, Player player)
    {
        this.Type = type;
        this.Card = card;
        this.Player = player;
    }
}
