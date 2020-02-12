﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : SingletonGameObject<ClientManager>
{
    private const int PORT_NUMBER = 3074;

    private Action _onConnectComplete;

    public ClientManager ConnectToServer()
    {
        IPAddress serverAddress = IPAddress.Parse("192.168.1.101");

        IPEndPoint serverEP = new IPEndPoint(serverAddress, PORT_NUMBER);

        Socket client = new Socket(serverAddress.AddressFamily,
                                     SocketType.Stream, ProtocolType.Tcp);

        client.BeginConnect(serverEP, new AsyncCallback(ConnectCallback), client);

        return this;
    }

    private void ConnectCallback(IAsyncResult re)
    {
        Socket server = re.AsyncState as Socket;

        if (server == null)
        {
            Debug.LogWarning("null server socket");
        }
        
        Debug.Log("connected?: " + !server.Poll(10, SelectMode.SelectRead));

        _onConnectComplete?.Invoke();
    }

    public ClientManager SetOnConnectComplete(Action onComplete)
    {
        _onConnectComplete = onComplete;

        return this;
    }
}