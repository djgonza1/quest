using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Silvermine.Battle.Core;

public class BoardSceneManager : SingletonGameObject<BoardSceneManager>, IBattleEventManager, IPlayer
{
    public const float CardOverSizePosOffset = 3f;
    public const float CardOverSizeScale = 3f;

    [SerializeField] private CardHandController _playerHand;
    [SerializeField] private CardHandController _enemyHand;
    [SerializeField] private Text _battleText = null;

    public ActionQueue CallbackQueue;
    private BoardSessionManager _session;

    public CardHandController PlayerHandController { get => _playerHand; }
    public CardHandController EnemyHandController { get => _enemyHand; }
    
    // Start is called before the first frame update
    void Start()
    {
        CallbackQueue = new ActionQueue();

        PlayerInfo playerOneInfo = new PlayerInfo();
        PlayerInfo playerTwoInfo = new PlayerInfo();

        Board gameBoard = new Board(playerOneInfo, playerTwoInfo);

        IPlayer player = this;
        IPlayer opponent = new OfflineAIPlayer(gameBoard, PlayerType.Second, this);
        _session = new BoardSessionManager(gameBoard, player, opponent, this);
        
        CallbackQueue = new ActionQueue();
        
        ContentManager.Instance.LoadAbilityCardsPrefabs(StartBoardGameSession);
    }

    private void StartBoardGameSession()
    {
        List<AbilityCard> playerCards = _session.GameBoard.GetPlayerHand(PlayerType.First);
        List<AbilityCard> enemyCards = _session.GameBoard.GetPlayerHand(PlayerType.Second);

        _playerHand.Init(playerCards, PlayerType.First);
        _enemyHand.Init(enemyCards, PlayerType.Second);

        _session.StartSession();
    }
    
    public void OnBoardOpen()
    {
        QueuedAction boardOpenStart = (boardOpenEnd) =>
        {
            _battleText.text = "GameStart";
            _battleText.gameObject.SetActive(true);

            // foreach (var cInfo in PlayerHandMap.Values)
            // {
            //     //TODO - Switch card states here
            // }

            DelayCall(2f, () =>
            {
                _battleText.gameObject.SetActive(false);
                boardOpenEnd();
            });
        };

        CallbackQueue.QueuedCall(boardOpenStart);
    }

    public void RequestCardChoice(Action<AbilityCard> onCardChosen)
    {
        QueuedAction chooseCardsStart = (chooseCardEnd) =>
        {
            AbilityCard firstChoice = null;

            Action<CardGOEvent> onPlayerCardChosen = null;
            onPlayerCardChosen = (msg) =>
            {
                if (msg.Player != PlayerType.First || msg.Type != CardGOEvent.EventType.CHOSEN)
                {
                    return;
                }

                Events.Instance.RemoveListener(onPlayerCardChosen);

                firstChoice = msg.CardObject.Card;
                onCardChosen(firstChoice);

                chooseCardEnd();
            };

            Events.Instance.AddListener(onPlayerCardChosen);
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
            AbilityCard card = _session.GameBoard.playerTwo.BattleChoice;
            PlayableCardBehaviour cardGO = _enemyHand.GetCard(card);
            cardGO.FlipCard(true);

            card = _session.GameBoard.playerOne.BattleChoice;
            cardGO = _playerHand.GetCard(card);
            cardGO.FlipCard(true);

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
