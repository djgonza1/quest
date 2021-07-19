using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFocusHolder : MonoBehaviour
{
    [SerializeField] public Transform FocusCardLocator;
    [SerializeField] PlayableCardBehaviour _focusedCard;

    public IEnumerator FocusCardAndChoseTargets(Action<PlayableCardBehaviour> callback = null)
    {
        yield return 0;

        callback?.Invoke(null);
    }
}
