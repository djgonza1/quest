using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public interface IBundledAsset
    {
        string AssetName { get; }
    }

    public interface IBattleEventManager
    {
        void BoardOpen();
        void RequestPlayerOneChoice(Action<AbilityCard> onCardsChosen);
        void RequestPlayerTwoChoice(Action<AbilityCard> onCardsChosen);
        void StartBattlePhase(Player winner);
    }

    public interface ICardGO
    {
        void SetSortingOrder(int order);
        void Highlight(bool enable);
    }

    public interface IOnlinePlayer
    {
        void OnOpponentFound();
    }
}
