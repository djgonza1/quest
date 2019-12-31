using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue
{
    private Queue<Action> _callbackQueue;

    public ActionQueue()
    {
        _callbackQueue = new Queue<Action>();
    }
    
    public void Add(Action<Action> action)
    {
        action = (callback) =>
        {
            
        };
    }
}
