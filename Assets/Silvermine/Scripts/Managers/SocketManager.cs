using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class SocketManager : MonoBehaviour
{
    private int PORT_NUMBER = 3074;

    private Socket _workSocket;
    
    // Start is called before the first frame update
    void Start()
    {
        string externalip = new WebClient().DownloadString("http://icanhazip.com");
        Debug.Log("externalip: " + externalip);

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
        listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

        //IPAddress externalAddress = IPAddress.Parse(externalip.Trim());
        
        //IPEndPoint remoteEP = new IPEndPoint(externalAddress, PORT_NUMBER);
        
        //Socket client = new Socket(externalAddress.AddressFamily,
        //                             SocketType.Stream, ProtocolType.Tcp);

        //client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AcceptCallback(IAsyncResult re)
    {
        Debug.Log("accepted");
    }

    private void ConnectCallback(IAsyncResult re)
    {
        Socket client = re as Socket;

        if (client == null)
        {
            Debug.LogWarning("null client socket");
        }

        

        Debug.Log("connected?: " + !client.Poll(10, SelectMode.SelectRead));
    }
}
