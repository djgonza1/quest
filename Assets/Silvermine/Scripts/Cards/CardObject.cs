using UnityEngine;
using Silvermine.Duel.Core;

public class CardObject : MonoBehaviour
{
    [SerializeField]
    private CardColor _color;
    [SerializeField]
    private int _damange;

    private BaseMagicCard _card;

    void Start()
    {
        _card = new BaseMagicCard(_color, _damange);
        Debug.Log("Card is " + _card.Color);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseEnter()
    {
        LeanTween.scale(this.gameObject, new Vector2(1.3f, 1.3f), 0.2f);
    }

    private void OnMouseExit()
    {
        LeanTween.scale(this.gameObject, new Vector2(1f, 1f), 0.2f);
    }
}
