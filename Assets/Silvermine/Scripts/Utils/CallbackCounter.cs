using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackCounter
{
    public int Count;

    public CallbackCounter()
    {
        this.Count = 0;
    }

    public CallbackCounter(int count)
    {
        this.Count = count;
    }

    public static CallbackCounter operator ++(CallbackCounter counter)
    {
        ++counter.Count;
        return counter;
    }

    public static CallbackCounter operator --(CallbackCounter counter)
    {
        --counter.Count;
        return counter;
    }
    
    public static implicit operator int(CallbackCounter counter)
    {
        return counter.Count;
    }
}
