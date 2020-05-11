using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : SingletonGameObject<ClientManager>
{
    private const int PORT_NUMBER = 11000;
    public Socket Client;

    private Action _onConnectComplete;

    public ClientManager ConnectToServer()
    {
        IPAddress serverAddress = IPAddress.Parse("192.168.1.102");

        IPEndPoint serverEP = new IPEndPoint(serverAddress, PORT_NUMBER);
        
        Debug.LogWarning("server family: " + serverAddress.AddressFamily);
        Client = new Socket(serverAddress.AddressFamily,
                                     SocketType.Stream, ProtocolType.Tcp);

        Client.BeginConnect(serverEP, new AsyncCallback(ConnectCallback), Client);

        return this;
    }

    private void ConnectCallback(IAsyncResult re)
    {
        Client = re.AsyncState as Socket;
        
        Debug.Log("connected?: " + !Client.Poll(10, SelectMode.SelectRead));

        Client.EndConnect(re);

        Client.BeginSend("HELLO");

        _onConnectComplete?.Invoke();
    }

    public ClientManager SetOnConnectComplete(Action onComplete)
    {
        _onConnectComplete = onComplete;

        return this;
    }
}
