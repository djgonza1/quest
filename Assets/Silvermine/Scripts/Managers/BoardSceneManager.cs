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
    [SerializeField] private Transform _leftSpellBoardLocator = null;
    [SerializeField] private Transform _rightSpellBoardLocator = null;
    [SerializeField] private Text _battleText = null;

    public ActionQueue CallbackQueue;
    private BoardSessionManager _session;
    private bool _playerOneCardChosen;
    private bool _playerTwoCardChosen;

    public CardHandController PlayerHand { get => _playerHand; }
    public CardHandController EnemyHand { get => _enemyHand; }
    
    // Start is called before the first frame update
    void Start()
    {
        _playerOneCardChosen = false;
        _playerTwoCardChosen = false;

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
        _playerHand.CreateHand(playerCards, PlayerType.First);

        List<AbilityCard> enemyCards = _session.GameBoard.GetPlayerHand(PlayerType.Second);
        _enemyHand.CreateHand(enemyCards, PlayerType.Second);

        _session.StartSession();
    }
    
    public Vector3 GetCardHandPosition(PlayableCardBehaviour cardGO)
    {

        if (_playerHand.ContainsCard(cardGO))
        {
            return _playerHand.GetCardHandPosition(cardGO);
        }

        if (_enemyHand.ContainsCard(cardGO))
        {
            return _enemyHand.GetCardHandPosition(cardGO);
        }

        Debug.LogError("No hand locator found for card object");
        return Vector2.zero;
    }

    public Vector3 GetBoardPlayPosition(PlayableCardBehaviour cardGO)
    {
        if (_playerHand.ContainsCard(cardGO))
        {
            return _leftSpellBoardLocator.position;
        }
        else if (_enemyHand.ContainsCard(cardGO))
        {
            return _rightSpellBoardLocator.position;
        }

        Debug.LogError("No play locator found for card object");
        return Vector2.zero;
    }

    public Vector2 GetHandCardScale()
    {
        return new Vector2(0.6f, 0.6f);
    }

    public Vector2 GetPlayBoardCardScale()
    {
        return GetHandCardScale();
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

                // foreach (var cInfo in PlayerHandMap.Values)
                // {
                //     //TODO - Change Card state here
                // }

                Events.Instance.RemoveListener(onPlayerCardChosen);

                firstChoice = msg.CardObject.Card;
                onCardChosen(firstChoice);

                chooseCardEnd();
            };

            // foreach(var handCard in PlayerHandMap)
            // {
                
            // }

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
            // foreach (var cInfo in PlayerHandMap.Values)
            // {
            //     //TODO - Change Card State Here
            // }

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

    public struct HandCardInfo
    {
        public PlayableCardBehaviour CardGO;
        public Vector3 HandPosition;
        public Vector2 HandScale;
        public SMStateMachine<PlayableCardBehaviour> StateMachine;

        public HandCardInfo(PlayableCardBehaviour cardGO, Vector3 handPosition, Vector2 handScale, SMStateMachine<PlayableCardBehaviour> stateMachine)
        {
            CardGO = cardGO;
            HandPosition = handPosition;
            HandScale = handScale;
            StateMachine = stateMachine;
        }
    }
}
