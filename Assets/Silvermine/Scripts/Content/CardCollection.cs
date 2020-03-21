using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardCollection", menuName = "ScriptableObjects/ContentCollection", order = 1)]
public class CardCollection : ScriptableObject
{
    public List<AssetReference> Cards;

}
