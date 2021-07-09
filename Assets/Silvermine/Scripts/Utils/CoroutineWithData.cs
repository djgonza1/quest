using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineWithData
{
    public object result;
    private IEnumerator _target;

    public CoroutineWithData(IEnumerator target) {
        this._target = target;
    }

    private IEnumerator Run() {
        while(_target.MoveNext()) {
            result = _target.Current;
            yield return result;
        }
    }
}
