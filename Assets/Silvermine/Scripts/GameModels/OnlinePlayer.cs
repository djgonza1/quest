using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;


public class OnlinePlayer : IOnlinePlayer
{
    private Socket _playerSocket;

    public OnlinePlayer(Socket playerSocket)
    {
        _playerSocket = playerSocket;
    }

    public void OnOpponentFound()
    {
        _playerSocket.BeginSend("FOUND_OPPONENT");
    }
}
