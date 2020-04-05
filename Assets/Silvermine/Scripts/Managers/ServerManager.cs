using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    private int PORT_NUMBER = 11000;

    private void Start()
    {
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

        //--ClientTesting
        // IPEndPoint serverEP = new IPEndPoint(IPAddress.Loopback, PORT_NUMBER);

        // Socket client = new Socket(AddressFamily.InterNetwork,
        //                            SocketType.Stream, ProtocolType.Tcp);

        // client.BeginConnect(serverEP, new AsyncCallback(ConnectCallback), client);
    }
    
    private void OnCientAccepted(IAsyncResult re)
    {
        Socket client = re.AsyncState as Socket;

        Debug.Log("accepted");
    }

    //--ClientTesting
    // private void ConnectCallback(IAsyncResult re)
    // {
    //     Socket server = re.AsyncState as Socket;

    //     if (server == null)
    //     {
    //         Debug.LogWarning("null server socket");
    //     }
        
    //     Debug.Log("connected?: " + !server.Poll(10, SelectMode.SelectRead));
    // }
}
