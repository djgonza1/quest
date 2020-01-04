using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackCounter
{
    public int Count;
    public Action OnComplete; 

    public CallbackCounter()
    {
        this.Count = 0;
    }

    public CallbackCounter(int count, Action onComplete)
    {
        this.Count = count;
        this.OnComplete = onComplete;
    }

    public static CallbackCounter operator ++(CallbackCounter counter)
    {
        ++counter.Count;
        return counter;
    }

    public static CallbackCounter operator --(CallbackCounter counter)
    {
        --counter.Count;

        if (counter.Count == 0)
        {
            counter.OnComplete();
        }

        return counter;
    }
    
    public static implicit operator int(CallbackCounter counter)
    {
        return counter.Count;
    }
}
