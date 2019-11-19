using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Silvermine.Battle.Core;

public class ContentManager : SingletonGameObject<ContentManager>
{
    private AssetBundle _cardsBundle;
    private GameObject _cardAsset;
    
    // Start is called before the first frame update
    void Start()
    {
        string cardBundlePath = GetCardsBundlePath();

        _cardsBundle = AssetBundle.LoadFromFile(cardBundlePath);
    }
    
    public CardObject LoadSpellCardObject(BaseMagicCard spellCard)
    {
        if (_cardAsset == null)
        {
            _cardAsset = _cardsBundle.LoadAsset<GameObject>(CardObject.AssetName);
        }
        
        var go = Instantiate(_cardAsset);

        CardObject cardObject = go.GetComponent<CardObject>();
        
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
