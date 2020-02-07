using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonGameObject<GameManager>
{
    private void OnEnable()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
