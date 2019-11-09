using UnityEngine;
using System.Collections.Generic;
using Silvermine.Battle.Core;

public class BoardSceneManager : SingletonGameObject<BoardSceneManager>
{
    [SerializeField]
    private Transform[] _cardLocators;
    [SerializeField]
    private Transform _leftSpellBoardLocator;
    [SerializeField]
    private Transform _rightSpellBoardLocator;

    private Dictionary<CardObject, Transform> _handMap;
    public BoardSessionManager _session;

    public CardObject tempCard;
    // Start is called before the first frame update
    void Start()
    {
        _session = new BoardSessionManager();

        _handMap = new Dictionary<CardObject, Transform>();

        BaseMagicCard card1 = new BaseMagicCard(CardColor.Red, 0);
        BaseMagicCard card2 = new BaseMagicCard(CardColor.Green, 0);
        BaseMagicCard card3 = new BaseMagicCard(CardColor.Blue, 0);

        CardObject cardObject1 = ContentManager.Instance.LoadSpellCardObject(card1);
        CardObject cardObject2 = ContentManager.Instance.LoadSpellCardObject(card2);
        CardObject cardObject3 = ContentManager.Instance.LoadSpellCardObject(card3);

        _handMap.Add(cardObject1, _cardLocators[0]);
        _handMap.Add(cardObject2, _cardLocators[1]);
        _handMap.Add(cardObject3, _cardLocators[2]);

        foreach(var pair in _handMap)
        {
            var card = pair.Key;
            var loc = pair.Value;

            card.transform.position = loc.transform.position;
        }

        Events.Instance.AddListener<CardEvent>(OnCardEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetCardHandPosition(CardObject card)
    {
        if (_handMap.ContainsKey(card))
        {
            return _handMap[card].position;
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
