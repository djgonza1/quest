using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void QueuedAction(Action onComplete);

public class ActionQueue
{
    private Queue<QueuedAction> _callbackQueue;

    public ActionQueue()
    {
        _callbackQueue = new Queue<QueuedAction>();
    }
    
    public ActionQueue QueuedCall(QueuedAction action)
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

        QueuedAction current = _callbackQueue.Peek();
        current(NextCallback);
    }
}
