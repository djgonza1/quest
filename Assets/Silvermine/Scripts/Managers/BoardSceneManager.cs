using UnityEngine;
using System.Collections.Generic;
using Silvermine.Battle.Core;

public class BoardSceneManager : SingletonGameObject<BoardSceneManager>
{
    [SerializeField]
    private CardObject[] _cards;
    
    [SerializeField]
    private Transform[] _cardLocators;
    
    private Dictionary<CardObject, Transform> _handMap;
    public BoardSessionManager _session;

    // Start is called before the first frame update
    void Start()
    {
        _session = new BoardSessionManager();

        _handMap = new Dictionary<CardObject, Transform>();
        for(int i = 0; i < _cards.Length; i++)
        {
            _handMap.Add(_cards[i], _cardLocators[i]);
        }

        Events.Instance.AddListener<CardEvent>(OnCardEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetCardHandPosition(CardObject card)
    {
        if (_handMap.ContainsKey(card))
        {
            return _handMap[card].position;
        }

        Debug.LogError("No hand locator found for card object");
        return Vector2.zero;
    }

    public Vector2 GetHandCardScale()
    {
        return new Vector2(0.6f, 0.6f);
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
