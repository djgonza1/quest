using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Duel.Core;

public class CardObject : MonoBehaviour
{
    [SerializeField]
    private CardColor _color;
    [SerializeField]
    private int _damange;

    private BaseMagicCard _card;

    void Start()
    {
        _card = new BaseMagicCard(_color, _damange);
        Debug.Log("Card is " + _card.Color);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
