﻿using System;
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

    public interface ICardBehavior
    {
        AbilityCard Card { get; }
        void SetSortingOrder(int order);
        void Highlight(bool enable);
    }
}
