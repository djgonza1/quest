using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public abstract class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private CardHandController _playerHand;
    public CardHandController HandController => _playerHand;

    public PlayerInfo Info { get; private set; }

    public void Init(PlayerInfo info, PlayerType playerType = PlayerType.First)
    {
        Info = info;
        _playerHand.Init(Info.Hand, playerType);
    }

    public IEnumerator PlayCard(Action<PlayableCardBehaviour> callback = null)
    {
        yield return WaitForCardPlayed();
        Debug.LogWarning("CardChosen");
        callback(CardChoice);
    }

    public abstract PlayableCardBehaviour CardChoice { get; }
    protected abstract IEnumerator WaitForCardPlayed();
}
