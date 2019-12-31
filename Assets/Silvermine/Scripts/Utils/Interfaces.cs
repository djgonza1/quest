using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public interface IBundledAsset
    {
        string AssetName { get; }
    }

    public interface ICardStateMachine
    {
        void OnCardEnter();
        void OnCardExit();
        void OnCardTapDown();
        void OnCardTapRelease();
        void OnCardHover();
        void OnCardDrag();
        void Update();
        void ChangeState<S>() where S : CardState;
    }

    public interface IBoardSceneManager
    {
        void BoardOpen(Action onComplete);
        void ChooseCards(Action<AbilityCard> choosePlayerOneCard, 
                             Action<AbilityCard> choosePlayerTwoCard,
                             Action onComplete);
        void StartBattlePhase(Player winner);
    }
}
