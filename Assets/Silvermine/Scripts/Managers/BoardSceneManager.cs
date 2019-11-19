using System;
using UnityEngine;
using System.Collections.Generic;
using Silvermine.Battle.Core;

public class BoardSceneManager : SingletonGameObject<BoardSceneManager>
{
    [SerializeField]
    private Transform[] _playerCardLocators;
    [SerializeField]
    private Transform[] _enemyCardLocators;
    [SerializeField]
    private Transform _leftSpellBoardLocator;
    [SerializeField]
    private Transform _rightSpellBoardLocator;

    private Dictionary<CardObject, Transform> _playerHandMap;
    private Dictionary<CardObject, Transform> _enemyHandMap;
    public BoardSessionManager _session;
    
    // Start is called before the first frame update
    void Start()
    {
        _session = new BoardSessionManager();

        BaseMagicCard[] playerCards =
        {
            new BaseMagicCard(CardColor.Red, 0),
            new BaseMagicCard(CardColor.Green, 0),
            new BaseMagicCard(CardColor.Blue, 0)
        };

        BaseMagicCard[] enemyCards =
        {
            new BaseMagicCard(CardColor.None, 0),
            new BaseMagicCard(CardColor.None, 0),
            new BaseMagicCard(CardColor.None, 0)
        };
        

        _playerHandMap = CreateHand(playerCards);
        _enemyHandMap = CreateHand(enemyCards, false);

        Events.Instance.AddListener<CardEvent>(OnCardEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Dictionary<CardObject, Transform> CreateHand(BaseMagicCard[] cards, bool isPlayerHand = true)
    {
        Dictionary<CardObject, Transform> handMap = new Dictionary<CardObject, Transform>();

        for (int i = 0; i < cards.Length; i++)
        {
            CardObject cardObject = ContentManager.Instance.LoadSpellCardObject(cards[i]);

            CardState[] states =
            {
                new InHand(),
                new Grabbed(),
            };

            CardStateMachine machine = new CardStateMachine(cardObject, states);

            cardObject.Init(cards[i], machine);

            Transform handLoc = isPlayerHand ? _playerCardLocators[i] : _enemyCardLocators[i];

            handMap.Add(cardObject, handLoc);
        }

        foreach (var pair in handMap)
        {
            var card = pair.Key;
            var loc = pair.Value;

            card.transform.position = loc.transform.position;
            card.transform.localScale = GetHandCardScale();
        }

        return handMap;
    }
    
    public Vector3 GetCardHandPosition(CardObject card)
    {
        if (_playerHandMap.ContainsKey(card))
        {
            return _playerHandMap[card].position;
        }

        if (_enemyHandMap.ContainsKey(card))
        {
            return _enemyHandMap[card].position;
        }

        Debug.LogError("No hand locator found for card object");
        return Vector2.zero;
    }

    public Vector3 GetBoardPlayPosition(CardObject card)
    {
        if (_leftSpellBoardLocator)
        {
            return _leftSpellBoardLocator.position;
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

    public void OnCardEvent(CardEvent e)
    {
        Debug.LogWarning("Handled event by " + e.card);
    }

    public bool TryPlayCard(BaseMagicCard card)
    {
        return _session.TryPlayCard(Player.First, card);
    }

    private void OnDestroy()
    {
        Events.Instance.RemoveListener<CardEvent>(OnCardEvent);
    }
}
