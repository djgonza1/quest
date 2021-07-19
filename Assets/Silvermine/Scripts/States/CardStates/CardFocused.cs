using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class CardFocused : SMState<PlayableCardBehaviour>
{
    public override void Begin()
    {
        Debug.LogWarning("Begin CardFocused");
    }
    
}
