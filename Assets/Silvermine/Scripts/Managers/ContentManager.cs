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
    private GameObject _cardAsset;

    // Start is called before the first frame update
    void Start()
    {
        _cardCollection.Cards[0].InstantiateAsync().Completed += (handle) =>
        {
            var go = handle.Result;
            var card = go.GetComponent<CardGO>();
            Debug.LogWarning("loaded: " + card.gameObject.name);
        };
    }
    
    public CardGO LoadSpellCardObject(AbilityCard spellCard)
    {
        //if (_cardAsset == null)
        //{
        //    _cardAsset = _cardsBundle.LoadAsset<GameObject>(CardGO.AssetName);
        //}
        
        //var go = Instantiate(_cardAsset);

        //CardGO cardObject = go.GetComponent<CardGO>();
        
        return null;
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
