using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeClientManager : SingletonGameObject<ClientManager>
{
    private const int PORT_NUMBER = 11000;

    public Socket Client;
    public event Action OnConnectComplete;
    public event Action<string> OnOpponentFound;

    private bool _connectCompleted;
    private bool _opponentFound;
    
    void Update()
    {
        if (_connectCompleted)
        {
            _connectCompleted = false;
            OnConnectComplete?.Invoke();
        }

        if (_opponentFound)
        {
            _opponentFound = false;
            //OnOpponentFound?.Invoke("name");
        }
    }

    public void ConnectToServer()
    {
        IPAddress serverAddress = IPAddress.Parse("192.168.1.102");

        IPEndPoint serverEP = new IPEndPoint(serverAddress, PORT_NUMBER);
        
        Debug.LogWarning("server family: " + serverAddress.AddressFamily);
        Client = new Socket(serverAddress.AddressFamily,
                                     SocketType.Stream, ProtocolType.Tcp);

        Client.BeginConnect(serverEP, new AsyncCallback(ConnectCallback), Client);
    }

    private void ConnectCallback(IAsyncResult re)
    {
        Client = re.AsyncState as Socket;
        
        Debug.Log("FAKE connected?: " + !Client.Poll(10, SelectMode.SelectRead));

        _connectCompleted = true;
        Client.EndConnect(re);
    }

    public void FindOpponent()
    {
        Client.BeginSend("FIND_OPPONENT", FoundCallback);
    }
    
    private void FoundCallback(IAsyncResult re)
    {
        Debug.LogWarning("FAKE FoundCallback");
        
        Client = re.AsyncState as Socket;

        OnOpponentFound?.Invoke("name");
    }
}
