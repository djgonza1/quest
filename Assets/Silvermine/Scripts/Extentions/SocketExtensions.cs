using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SocketExtensions
{
    public static void BeginSend(this Socket socket, String data, AsyncCallback callback)
    {
        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.  
        socket.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(callback), socket);
    }
}
