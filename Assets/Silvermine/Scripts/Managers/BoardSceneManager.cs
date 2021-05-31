using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Silvermine.Battle.Core;

public class BoardSceneManager : MonoBehaviour, IPlayer
{
    public const float CardOverSizePosOffset = 3f;
    public const float CardOverSizeScale = 3f;

    [SerializeField] private CardHandController _playerHand;
    [SerializeField] private CardHandController _enemyHand;
    [SerializeField] private Text _battleText = null;
    [SerializeField] private BoardStateMachine _boardStateMachine;

    public BoardSessionManager Session { get; private set; }
    public IPlayer Player { get; private set; }
    public PlayerInfo Info { get; private set; }
    public IPlayer Enemy { get; private set; }
    public PlayableCardBehaviour PlayerOneChoice;
    public PlayableCardBehaviour PlayerTwoChoice;
    public CardHandController PlayerHandController { get => _playerHand; }
    public CardHandController EnemyHandController { get => _enemyHand; }

    // Start is called before the first frame update
    void Start()
    {

        Info = new PlayerInfo();
        PlayerInfo playerTwoInfo = new PlayerInfo();

        Board gameBoard = new Board(Info, playerTwoInfo);

        Player = this;
        Enemy = new OfflineAIPlayer(gameBoard, playerTwoInfo, this);
        Session = new BoardSessionManager(gameBoard, Player, Enemy);
        
        ContentManager.Instance.LoadAbilityCardsPrefabs(StartBoardGameSession);
    }

    private void StartBoardGameSession()
    {
        List<AbilityCard> playerCards = Session.GameBoard.GetPlayerHand(PlayerType.First);
        List<AbilityCard> enemyCards = Session.GameBoard.GetPlayerHand(PlayerType.Second);

        _playerHand.Init(playerCards, PlayerType.First);
        _enemyHand.Init(enemyCards, PlayerType.Second);

        _boardStateMachine.Init();
    }
    
    public IEnumerator OpenBoard()
    {
        float openDuration = 2f;

        _battleText.text = "GameStart";
        _battleText.gameObject.SetActive(true);

        yield return new WaitForSeconds(openDuration);

        _battleText.gameObject.SetActive(false);
    }

    public void RequestCardChoice(Action<AbilityCard> onCardChosen)
    {
        Action<AbilityCard> onPlayerCardChosen = null;
        onPlayerCardChosen = (card) =>
        {
            _playerHand.OnCardChosen -= onPlayerCardChosen;

            PlayerOneChoice = PlayerHandController.GetCard(card);

            onCardChosen(card);

        };

        _playerHand.OnCardChosen += onPlayerCardChosen;
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
        PlayerOneChoice.FlipCard(true);

        yield return new WaitForSeconds(1f);
    }
}
