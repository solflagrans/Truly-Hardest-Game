using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Waste : MonoBehaviour, IDropHandler
{

    //—брос, где хран€тс€ все карты.

    [field: SerializeField] public RectTransform RectTransform { get; private set; }

    List<ICard> _cards = new List<ICard>();

    public event Action WasteUpdated;

    private void OnValidate() {
        
        if(RectTransform == null) RectTransform = GetComponent<RectTransform>();

    }

    public void AddCard(ICard card) {

        _cards.Add(card);

        WasteUpdated?.Invoke();

    }

    public void OnDrop(PointerEventData eventData) {
        if(eventData.pointerDrag.TryGetComponent(out ICard card)) {
            card.MoveToWaste();
        }
    }

    public void RemoveLastCard() {

        if(GetLastCard() != null) _cards.Remove(GetLastCard());

        WasteUpdated?.Invoke();

    }

    public ICard GetLastCard() {

        if(_cards.Count == 0) return null;

        return _cards[_cards.Count - 1];

    }

    public sbyte GetPeakCardsCount() {

        sbyte i = 0;

        foreach(ICard card in _cards) {
            if(card.GetType() == typeof(PeakCard)) {
                i++;
            }
        }

        return i;

    }

}
