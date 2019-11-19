using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class Grabbed : CardState
{
    public override void Begin()
    {
        _context.GrabCard();
    }

    public override void Update()
    {

    }

    public override void End()
    {

    }
}
