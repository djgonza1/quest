using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SocketExtensions
{
    public static void BeginSend(this Socket socket, String data, AsyncCallback callback = null)
    {
        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        AsyncCallback onComplete = (ar)=>
        {
            try 
            {  
                // Retrieve the socket from the state object.  
                Socket client = (Socket) ar.AsyncState;  
    
                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);  
                Debug.Log("Sent " + bytesSent + " bytes to server.");  
    
                callback.Invoke(ar);  
            } 
            catch (Exception e) 
            {  
                Console.WriteLine(e.ToString());  
            } 
        };

        // Begin sending the data to the remote device.  
        socket.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(onComplete), socket);
    }
}
