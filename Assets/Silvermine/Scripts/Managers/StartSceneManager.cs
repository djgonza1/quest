using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : SingletonGameObject<StartSceneManager>
{
    [SerializeField]
    private Button _vsAIbutton;
    [SerializeField]
    private Button _onlineButton;
    [SerializeField]
    private Text _onlineText;

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
            LoadBoardScene();
        }
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

        ClientManager.Instance.ConnectToServer()
                              .SetOnConnectComplete(()=> {
                                  _loadBoard = true;
                              });
    }

    private void LoadBoardScene()
    {
        SceneManager.LoadScene("board_scene");
    }
}
