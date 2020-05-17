using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : SingletonGameObject<StartSceneManager>
{
    [SerializeField] private Button _vsAIbutton = null;
    [SerializeField] private Button _onlineButton = null;
    [SerializeField] private Text _onlineText = null;

    private bool _loadBoard;

    private void Start()
    {
        _loadBoard = false;
        _onlineText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_loadBoard)
        {
            SceneManager.LoadScene("board_scene");
        }

        _loadBoard = false;
    }

    public void OnVsAIButtonClick()
    {
        _loadBoard = true;
    }

    public void OnOnlineButtonClick()
    {
        _vsAIbutton.gameObject.SetActive(false);
        _onlineButton.gameObject.SetActive(false);
        
        _onlineText.gameObject.SetActive(true);
        _onlineText.text = "Connecting to Server...";

        ClientManager.Instance.OnConnectComplete += ()=> 
        {
            Debug.LogWarning("Client OnConnectComplete");
            _onlineText.text = "Finding Opponent...";
            ClientManager.Instance.FindOpponent();
        };

        ClientManager.Instance.OnOpponentFound += (name)=>
        {
            _loadBoard = true;
        };

        ClientManager.Instance.ConnectToServer();
    }
}
