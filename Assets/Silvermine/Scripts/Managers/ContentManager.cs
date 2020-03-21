using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.IO;
using Silvermine.Battle.Core;

public class ContentManager : SingletonGameObject<ContentManager>
{
    [SerializeField]
    private CardCollection _cardCollection;
    private GameObject _cardPrefab;
    
    public void LoadAbilityCardsPrefabs(Action onComplete = null)
    {
        var operation = _cardCollection.Cards[0].LoadAssetAsync<GameObject>();

        operation.Completed += (handle) =>
        {
            _cardPrefab = handle.Result;

            var cardColors = Enum.GetValues(typeof(CardColor));
            
            onComplete?.Invoke();
        };
    }
    
    public PlayableCardGO CreateCardObject(AbilityCard card)
    {
        var go = Instantiate(_cardPrefab.gameObject);

        PlayableCardGO cardObject = go.GetComponent<PlayableCardGO>();
        cardObject.Init(card);

        return cardObject;
    }

    public static string GetAssetBundlesPath()
    {
        return Path.Combine(Application.dataPath, "AssetBundles");
    }

    public static string GetCardsBundlePath()
    {
        return Path.Combine(GetAssetBundlesPath(), "gameobjects/cards");
    }
}
