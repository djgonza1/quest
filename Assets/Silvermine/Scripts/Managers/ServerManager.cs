using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : SingletonGameObject<ServerManager>
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

    private int PORT_NUMBER = 11000;
    private Queue<KeyValuePair<Socket,string>> _messageQueue;

    public event Action<Socket, string> OnMessageReceived;

    private void Start()
    {
        _messageQueue = new Queue<KeyValuePair<Socket,string>>();

        IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress localAddress = null;

        foreach (var ip in hostInfo.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                Debug.Log("ip: " + ip);
                localAddress = ip;
                break;
            }
        }

        Debug.Log("hostEntry: " + Dns.GetHostName());

        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, PORT_NUMBER);

        Debug.LogWarning("AddressFamily: " + localAddress.AddressFamily);
        Socket listener = new Socket(localAddress.AddressFamily,
                                     SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(localEndPoint);
        listener.Listen(100);
        listener.BeginAccept(new AsyncCallback(OnCientAccepted), listener);
        //listener.BeginAccept(new AsyncCallback(OnCientAccepted), listener);

        //--ClientTesting
        // IPEndPoint serverEP = new IPEndPoint(IPAddress.Loopback, PORT_NUMBER);

        // Socket client = new Socket(AddressFamily.InterNetwork,
        //                            SocketType.Stream, ProtocolType.Tcp);

        // client.BeginConnect(serverEP, new AsyncCallback(ConnectCallback), client);
    }

    private void Update()
    {
        while (_messageQueue.Count > 0)
        {
            var pair = _messageQueue.Dequeue();
            var socket = pair.Key;
            var message = pair.Value;

            OnMessageReceived?.Invoke(socket, message);
        }
    }
    
    private void OnCientAccepted(IAsyncResult re)
    {
        Socket listener = re.AsyncState as Socket;
        Socket handler = listener.EndAccept(re);  
  
        // Create the state object.  
        StateObject state = new StateObject();  
        state.workSocket = handler;

        handler.BeginReceive( state.buffer, 0, 256, 0,  
            new AsyncCallback(ReadCallback), state);

        Debug.Log("SERVER accepted client");
        listener.BeginAccept(new AsyncCallback(OnCientAccepted), listener);
    }

    private void ReadCallback( IAsyncResult ar ) 
    {  
        try 
        {  
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

                Debug.Log("SERVER received message: " + message);  

                var pair = new KeyValuePair<Socket,string>(client, message);
                _messageQueue.Enqueue(pair);
  
                // Get the rest of the data.  
                client.BeginReceive(state.buffer,0,StateObject.BufferSize,0,  
                    new AsyncCallback(ReadCallback), state);  
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
}
