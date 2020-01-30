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

    private void Start()
    {
        _onlineText.gameObject.SetActive(false);
    }

    public void OnVsAIButtonClick()
    {
        SceneManager.LoadScene("board_scene");
    }

    public void OnOnlineButtonClick()
    {
        _vsAIbutton.gameObject.SetActive(false);
        _onlineButton.gameObject.SetActive(false);

        _onlineText.gameObject.SetActive(true);
        _onlineText.text = "Connecting to Server...";
    }
}
