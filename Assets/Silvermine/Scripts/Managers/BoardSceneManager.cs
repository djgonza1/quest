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

    [SerializeField] private Transform[] _playerCardLocators = null;
    [SerializeField] private Transform[] _enemyCardLocators = null;
    [SerializeField] private Transform _leftSpellBoardLocator = null;
    [SerializeField] private Transform _rightSpellBoardLocator = null;
    [SerializeField] private Text _battleText = null;

    public CallbackQueue CallbackQueue;

    public Dictionary<AbilityCard, CardGOSceneInfo> PlayerHandMap;
    public Dictionary<AbilityCard, CardGOSceneInfo> EnemyHandMap;
    private BoardSessionManager _session;
    private bool _playerOneCardChosen;
    private bool _playerTwoCardChosen;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerOneCardChosen = false;
        _playerTwoCardChosen = false;

        CallbackQueue = new CallbackQueue();

        PlayerInfo playerOneInfo = new PlayerInfo();
        PlayerInfo playerTwoInfo = new PlayerInfo();

        Board gameBoard = new Board(playerOneInfo, playerTwoInfo);

        IPlayer player = this;
        IPlayer opponent = new OfflineAIPlayer(gameBoard, PlayerType.Second, this);
        _session = new BoardSessionManager(gameBoard, player, opponent, this);
        
        CallbackQueue = new CallbackQueue();
        
        ContentManager.Instance.LoadAbilityCardsPrefabs(StartBoardGameSession);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHandMap == null)
        {
            return;
        }

        foreach (var cardInfo in PlayerHandMap.Values)
        {
            cardInfo.StateMachine.Update();
        }
    }

    private void StartBoardGameSession()
    {
        PlayerHandMap = CreateHand(_session.GameBoard.GetPlayerHand(PlayerType.First));
        EnemyHandMap = CreateHand(_session.GameBoard.GetPlayerHand(PlayerType.Second), false);
        
        _session.StartSession();
    }

    private Dictionary<AbilityCard, CardGOSceneInfo> CreateHand(List<AbilityCard> cards, bool isPlayerHand = true)
    {
        Dictionary<AbilityCard, CardGOSceneInfo> handMap = new Dictionary<AbilityCard, CardGOSceneInfo>();
        
        for (int i = 0; i < cards.Count; i++)
        {
            PlayableCardGO cardObject = ContentManager.Instance.CreateCardObject(cards[i]);

            cardObject.IsTappable = isPlayerHand;
            cardObject.FlipCard(isPlayerHand);

            Transform handLoc = isPlayerHand ? _playerCardLocators[i] : _enemyCardLocators[i];

            SMState<PlayableCardGO>[] states =
            {
                new InHand(),
                new Grabbed(),
                new InPlay()
            };

            SMStateMachine<PlayableCardGO> machine = new SMStateMachine<PlayableCardGO>(cardObject, states);

            CardGOSceneInfo cardInfo = new CardGOSceneInfo(cardObject, handLoc.position, machine);

            handMap.Add(cards[i], cardInfo);
        }

        foreach (var cInfo in handMap.Values)
        {
            var position = cInfo.HandPosition;

            cInfo.CardGO.transform.position = position;
            cInfo.CardGO.transform.localScale = GetHandCardScale();
        }

        return handMap;
    }
    
    public Vector3 GetCardHandPosition(PlayableCardGO cardGO)
    {

        if (PlayerHandMap.ContainsKey(cardGO.Card))
        {
            return PlayerHandMap[cardGO.Card].HandPosition;
        }

        if (EnemyHandMap.ContainsKey(cardGO.Card))
        {
            return EnemyHandMap[cardGO.Card].HandPosition;
        }

        Debug.LogError("No hand locator found for card object");
        return Vector2.zero;
    }

    public Vector3 GetBoardPlayPosition(PlayableCardGO cardGO)
    {
        if (PlayerHandMap.ContainsKey(cardGO.Card))
        {
            return _leftSpellBoardLocator.position;
        }
        else if (EnemyHandMap.ContainsKey(cardGO.Card))
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
        Action<Action> boardOpenStart = (boardOpenEnd) =>
        {
            _battleText.text = "GameStart";
            _battleText.gameObject.SetActive(true);

            foreach (var cInfo in PlayerHandMap.Values)
            {
                cInfo.CardGO.IsTappable = false;
            }

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
        Action<Action> chooseCardsStart = (chooseCardEnd) =>
        {
            AbilityCard firstChoice = null;

            Action<CardGOEvent> onPlayerCardChosen = null;
            onPlayerCardChosen = (msg) =>
            {
                if (msg.Player != PlayerType.First || msg.Type != CardGOEvent.EventType.CHOSEN)
                {
                    return;
                }

                foreach (var cInfo in PlayerHandMap.Values)
                {
                    cInfo.CardGO.IsTappable = false;
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
            foreach (var cInfo in PlayerHandMap.Values)
            {
                cInfo.CardGO.IsTappable = true;
            }

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
        Action<Action> phaseStart = (phaseEnd) =>
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

        Action<Action> flipStart = (flipEnd) =>
        {
            AbilityCard card = _session.GameBoard.playerTwo.BattleChoice;
            PlayableCardGO cardGO = EnemyHandMap[card].CardGO;
            cardGO.FlipCard(true);

            card = _session.GameBoard.playerOne.BattleChoice;
            cardGO = PlayerHandMap[card].CardGO;
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

    public struct CardGOSceneInfo
    {
        public PlayableCardGO CardGO;
        public Vector3 HandPosition;
        public SMStateMachine<PlayableCardGO> StateMachine;

        public CardGOSceneInfo(PlayableCardGO cardGO, Vector3 handPosition, SMStateMachine<PlayableCardGO> stateMachine)
        {
            CardGO = cardGO;
            HandPosition = handPosition;
            StateMachine = stateMachine;
        }
    }
}
