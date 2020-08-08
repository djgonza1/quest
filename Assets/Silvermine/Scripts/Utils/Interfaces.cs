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
        void OnBoardOpen();
        void OnChoosingPhaseStart();
        void OnBattlePhaseStart(PlayerType winner);
    }

    public interface ICardGO
    {
        void SetSortingOrder(int order);
        void Highlight(bool enable);
    }

    public interface IPlayer
    {
        void RequestCardChoice(Action<AbilityCard> onCardChosen);
    }

    public interface IOnlinePlayer
    {
        void OnOpponentFound();
    }
}
