﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeClientManager : SingletonGameObject<ClientManager>
{
    private class StateObject 
    {  
        // Client socket.  
        public Socket workSocket = null;  
        // Size of receive buffer.  
        public const int BufferSize = 256;  
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];  
        // Received data string.  
        public StringBuilder sb = new StringBuilder();  
    }  

    private const int PORT_NUMBER = 11000;

    public Socket Client;
    public event Action OnConnectComplete;
    public event Action<string> OnOpponentFound;

    private bool _connectSuccess;
    private bool _opponentFound;

    void Start()
    {
        Events.Instance.AddListener<OnlineEvent>(OnOnlineEvent);
    }
    
    void Update()
    {
        if (_connectSuccess)
        {
            _connectSuccess = false;
            Events.Instance.Raise<OnlineEvent>(new OnlineEvent(OnlineEvent.EventType.JOINED_SERVER));
        }

        if (_opponentFound)
        {
            _opponentFound = false;
            Events.Instance.Raise<OnlineEvent>(new OnlineEvent(OnlineEvent.EventType.FOUND_OPPONENT));
        }
    }

    public void ConnectToServer()
    {
        IPAddress serverAddress = IPAddress.Parse("192.168.1.102");

        IPEndPoint serverEP = new IPEndPoint(serverAddress, PORT_NUMBER);
        
        Client = new Socket(serverAddress.AddressFamily,
                                     SocketType.Stream, ProtocolType.Tcp);

        Client.BeginConnect(serverEP, new AsyncCallback(ConnectCallback), Client);
    }

    private void ConnectCallback(IAsyncResult re)
    {
        Client = re.AsyncState as Socket;
        Client.EndConnect(re);
        

        _connectSuccess = !Client.Poll(10, SelectMode.SelectRead);

        Debug.Log("ORIGINAL connected?: " + _connectSuccess);

        if (!_connectSuccess)
        {
            Debug.LogError("Failed to join server");
        }
    }

    public void FindOpponent()
    {
        Client.BeginSend("FIND_OPPONENT", FoundCallback);

        StateObject state = new StateObject();  
        state.workSocket = Client;

        Client.BeginReceive( state.buffer, 0, 256, 0,  
            new AsyncCallback(ReadCallback), state);
    }
    
    private void FoundCallback(IAsyncResult re)
    {
        Client = re.AsyncState as Socket;

        OnOpponentFound?.Invoke("name");
    }

    private void ReadCallback(IAsyncResult ar) 
    {  
        try {  
            // Retrieve the state object and the client socket
            // from the asynchronous state object.  
            StateObject state = (StateObject) ar.AsyncState;  
            Socket client = state.workSocket;  
  
            // Read data from the remote device.  
            int bytesRead = client.EndReceive(ar);  
  
            if (bytesRead > 0)
             {  
                // There might be more data, so store the data received so far.  
                //state.sb.Append(Encoding.ASCII.GetString(state.buffer,0,bytesRead));  

                string message = Encoding.ASCII.GetString(state.buffer,0,bytesRead);

                Debug.Log("CLIENT recieved message: " + message);  

                if (message == "FOUND_OPPONENT")
                {
                    _opponentFound = true;
                }
            }
            else
            {
                Debug.LogWarning("Received empty bytes");
            } 
        } 
        catch (Exception e) 
        {  
            Debug.LogException(e);  
        }  
    }

    private void OnOnlineEvent(OnlineEvent e)
    {
        switch (e.Type)
        {
            case OnlineEvent.EventType.REQUEST_SERVER_JOIN:
                ConnectToServer();
                break;
            case OnlineEvent.EventType.REQUEST_FIND_OPPONENT:
                FindOpponent();
                break;
            default:
                break;
        }
    }
}