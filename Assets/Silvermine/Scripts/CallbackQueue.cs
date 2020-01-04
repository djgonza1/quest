using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackQueue
{
    private Queue<Action<Action>> _callbackQueue;

    public CallbackQueue()
    {
        _callbackQueue = new Queue<Action<Action>>();
    }
    
    public CallbackQueue QueuedCall(Action<Action> action)
    {
        _callbackQueue.Enqueue(action);
        
        if (_callbackQueue.Count == 1)
        {
            action(NextCallback);
        }

        return this;
    }

    private void NextCallback()
    {
        _callbackQueue.Dequeue();

        if (_callbackQueue.Count == 0)
        {
            return;
        }

        Action<Action> current = _callbackQueue.Peek();
        current(NextCallback);
    }
}
