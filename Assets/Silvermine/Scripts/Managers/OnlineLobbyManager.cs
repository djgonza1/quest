using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class OnlineLobbyManager : MonoBehaviour
{
    private Dictionary<Socket, IOnlinePlayer> _waitingPlayers;
    private Dictionary<IOnlinePlayer, IOnlinePlayer> _matchedPlayers;

    // Start is called before the first frame update
    void Start()
    {
        _waitingPlayers = new Dictionary<Socket, IOnlinePlayer>();
        _matchedPlayers = new Dictionary<IOnlinePlayer, IOnlinePlayer>();

        ServerManager.Instance.OnMessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(Socket client, string message)
    {
        if (message == "FIND_OPPONENT")
        {
            if (_waitingPlayers.Count > 0)
            {
                var pair = _waitingPlayers.First();
                var opponent = pair.Value;

                _waitingPlayers.Remove(pair.Key);

                var player = new OnlinePlayer(client);
                //matched opponents keep track of eachother
                _matchedPlayers[player] = opponent;
                _matchedPlayers[opponent] = player;

                player.OnOpponentFound();  
                opponent.OnOpponentFound(); 
                
            }
            else
            {
                var player = new OnlinePlayer(client);
                //_waitingPlayers.Add(client, player);

                var opponent = new OnlineAIPlayer();

                _matchedPlayers[player] = opponent;
                _matchedPlayers[opponent] = player;

                player.OnOpponentFound();  
                opponent.OnOpponentFound();  
                
                Debug.LogWarning("Server matched client to AI opponent");
            }
        }
    }
}
