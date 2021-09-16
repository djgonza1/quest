using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Silvermine.Battle.Core;

public class BoardSceneManager : MonoBehaviour
{
    public const float CardOverSizePosOffset = 3f;
    public const float CardOverSizeScale = 3f;

    [SerializeField] private PlayerBehavior     _mainPlayer;
    [SerializeField] private PlayerBehavior     _enemyPlayer;
    [SerializeField] private Text               _battleText = null;
    [SerializeField] List<Transform> _selectedCardLocators;

    public BoardSessionManager                      Session { get; private set; }
    public Dictionary<PlayerInfo, PlayerBehavior>   Players { get; private set; }
    public PlayerBehavior                           CurrentPlayer => Players[Session.CurrentTurnPlayer];

    // Start is called before the first frame update
    void Start()
    {

        PlayerInfo playerOneInfo = new PlayerInfo();
        PlayerInfo playerTwoInfo = new PlayerInfo();
        Session = new BoardSessionManager(playerOneInfo, playerTwoInfo);

        Players = new Dictionary<PlayerInfo, PlayerBehavior>();
        Players.Add(playerOneInfo, _mainPlayer);
        Players.Add(playerTwoInfo, _enemyPlayer);
        
        ContentManager.Instance.LoadAbilityCardsPrefabs(StartBoardGameSession);
    }

    private void StartBoardGameSession()
    {
        // List<AbilityCard> playerCards = Session.GameBoard.GetPlayerHand(PlayerType.First);
        // List<AbilityCard> enemyCards = Session.GameBoard.GetPlayerHand(PlayerType.Second);

        _mainPlayer.Init(Session.PlayerOne, PlayerType.First);
        _enemyPlayer.Init(Session.PlayerTwo, PlayerType.Second);

        StartCoroutine(BeginSession());
    }

    private IEnumerator BeginSession()
    {
        yield return OpenBoard();

        while (true)
        {
            yield return WaitForPlayerToChooseCard();

            Debug.LogWarning("Starting BattlePhase");
            yield return BattlePhaseStart();
        }
    }
    
    public IEnumerator OpenBoard()
    {
        float openDuration = 2f;

        _battleText.text = "GameStart";
        _battleText.gameObject.SetActive(true);

        yield return new WaitForSeconds(openDuration);

        _battleText.gameObject.SetActive(false);
    }

    public IEnumerator WaitForPlayerToChooseCard()
    {
        Debug.LogWarning("Starting ChoosingPhase");
        yield return OpenChooseCardPopup();
        
        var currentPlayer = CurrentPlayer;

        PlayableCardBehaviour cardBehavior = null;
        yield return currentPlayer.PlayCard((card)=> { cardBehavior = card; });

        Debug.LogWarning("After chose card in state machine");

        Session.SetPlayerChoice(currentPlayer.Info, cardBehavior.Card);

        yield return new WaitForSeconds(5f);
    }

    public IEnumerator OpenChooseCardPopup()
    {
        _battleText.text = "Choose Card";
        _battleText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        _battleText.gameObject.SetActive(false);
    }

    public IEnumerator BattlePhaseStart()
    {
        _battleText.text = "Flip Cards";
        _battleText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        _battleText.gameObject.SetActive(false);

        CurrentPlayer.CardChoice.FlipCard(true);

        yield return new WaitForSeconds(1f);
    }
}
