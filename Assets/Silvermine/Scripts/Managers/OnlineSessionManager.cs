using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineSessionManager
{
    private enum SessionState { Lobby, BoardMatch };

    private SessionState CurrentState;

    public OnlineSessionManager()
    {
        CurrentState = SessionState.Lobby;
    }
}
