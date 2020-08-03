using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class OnlineAIPlayer : IOnlinePlayer
{
    public OnlineAIPlayer()
    {

    }

    public void OnOpponentFound()
    {
        Debug.LogWarning("ONLINE AI READY TO FIGHT");
    }
}
