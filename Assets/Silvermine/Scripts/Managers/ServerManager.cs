using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    private int PORT_NUMBER = 3074;

    private void Start()
    {
        IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress localAddress = IPAddress.Parse("192.168.1.101");

        foreach (var ip in hostInfo.AddressList)
        {
            Debug.Log("ip: " + ip);
        }

        Debug.Log("hostEntry: " + Dns.GetHostName());

        IPEndPoint localEndPoint = new IPEndPoint(localAddress, PORT_NUMBER);

        Debug.LogWarning("AddressFamily: " + localAddress.AddressFamily);
        Socket listener = new Socket(localAddress.AddressFamily,
                                     SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(localEndPoint);
        listener.Listen(100);
        listener.BeginAccept(new AsyncCallback(OnCientAccepted), listener);
    }
    
    private void OnCientAccepted(IAsyncResult re)
    {
        Socket client = re.AsyncState as Socket;

        Debug.Log("accepted");
    }
}
