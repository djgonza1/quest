using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Silvermine.Battle.Core;

public class BoardSceneManager : MonoBehaviour, IBattleEventManager, IPlayer
{
    public const float CardOverSizePosOffset = 3f;
    public const float CardOverSizeScale = 3f;

    [SerializeField] private CardHandController _playerHand;
    [SerializeField] private CardHandController _enemyHand;
    [SerializeField] private Text _battleText = null;
    [SerializeField] private BoardStateMachine _boardStateMachine;

    public BoardSessionManager Session { get; private set; }
    public IPlayer Player { get; private set; }
    public IPlayer Enemy { get; private set; }
    public PlayableCardBehaviour PlayerOneChoice;
    public PlayableCardBehaviour PlayerTwoChoice;
    public CardHandController PlayerHandController { get => _playerHand; }
    public CardHandController EnemyHandController { get => _enemyHand; }
    public ActionQueue CallbackQueue;

    
    // Start is called before the first frame update
    void Start()
    {
        CallbackQueue = new ActionQueue();

        PlayerInfo playerOneInfo = new PlayerInfo();
        PlayerInfo playerTwoInfo = new PlayerInfo();

        Board gameBoard = new Board(playerOneInfo, playerTwoInfo);

        Player = this;
        Enemy = new OfflineAIPlayer(gameBoard, PlayerType.Second, this);
        Session = new BoardSessionManager(gameBoard, Player, Enemy, this);
        
        CallbackQueue = new ActionQueue();
        
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
    
    public void OnBoardOpen()
    {
        QueuedAction boardOpenStart = (boardOpenEnd) =>
        {
            _battleText.text = "GameStart";
            _battleText.gameObject.SetActive(true);

            DelayCall(2f, () =>
            {
                _battleText.gameObject.SetActive(false);
                boardOpenEnd();
            });
        };

        CallbackQueue.QueuedCall(boardOpenStart);
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
        QueuedAction chooseCardsStart = (chooseCardEnd) =>
        {
            Action<AbilityCard> onPlayerCardChosen = null;
            onPlayerCardChosen = (card) =>
            {
                _playerHand.OnCardChosen -= onPlayerCardChosen;

                PlayerOneChoice = PlayerHandController.GetCard(card);

                onCardChosen(card);

                chooseCardEnd();
            };

            _playerHand.OnCardChosen += onPlayerCardChosen;
        };

        CallbackQueue.QueuedCall(chooseCardsStart);
    }

    private void OpenChooseCardPopup(Action popupEnd)
    {
        _battleText.text = "Choose Card";
        _battleText.gameObject.SetActive(true);

        DelayCall(2f, () =>
        {
            _battleText.gameObject.SetActive(false);
            popupEnd();
        });
    }

    public void OnChoosingPhaseStart()
    {
        CallbackQueue.QueuedCall(OpenChooseCardPopup);
    }

    public void OnBattlePhaseStart(PlayerType winner)
    {
        QueuedAction phaseStart = (phaseEnd) =>
        {
            _battleText.text = "Flip Cards";
            _battleText.gameObject.SetActive(true);

            DelayCall(2f, () =>
            {
                _battleText.text = "Flip Cards";
                _battleText.gameObject.SetActive(false);
                phaseEnd();
            });
        };

        QueuedAction flipStart = (flipEnd) =>
        {
            PlayerOneChoice.FlipCard(true);
            PlayerTwoChoice.FlipCard(true);

            DelayCall(1f, () =>
            {
                flipEnd();
            });
        };

        CallbackQueue.QueuedCall(phaseStart)
                     .QueuedCall(flipStart);
    }

    public void DelayCall(float delay, Action callback)
    {
        StartCoroutine(DelayedAction(delay, callback));
    }
    
    private IEnumerator DelayedAction(float delay, Action callback)
    {
        if(delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        callback.Invoke();
    }
}
