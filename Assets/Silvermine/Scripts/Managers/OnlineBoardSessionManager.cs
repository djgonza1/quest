using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class OnlineBoardSessionManager
{
    private BoardSessionManager _boardSession;

    public OnlineBoardSessionManager(Socket clientOne, Socket clientTwo)
    {
        clientOne.BeginSend("hello", SendCallBack);
    }

    private void SendCallBack(IAsyncResult re)
    {
        //TODO - add send logic here
    }
}
