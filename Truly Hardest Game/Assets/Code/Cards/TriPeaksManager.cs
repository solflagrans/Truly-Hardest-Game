using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;


public class TriPeaksManager : MonoBehaviour
{

    //Через него общаются карты, колода, стопка и сброс.
    //Содержит настройки игры и управляет её ходом.

    public static TriPeaksManager Instance;

    [Header("Instances")]
    [SerializeField] Deck _deck;
    [SerializeField] Waste _waste;
    [SerializeField] DeckEditor _editor;
    [SerializeField] TextMeshProUGUI _winText;
    [SerializeField] Score _score;
    [field: SerializeField] public AudioSource AudioSource { get; private set; }

    [Header("Settings")]
    [SerializeField] CardSettings[] _possibleCards;
    [field: SerializeField] public Sprite ClosedCardSprite { get; private set; }
    [SerializeField] AudioClip _cardSound;

    List<CardSettings> _minimumRequiredCards = new List<CardSettings>();

    public bool InEditMode { get; set; }
    public ICard SelectedCard { get; set; }

    private void Awake() {
        
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(Instance);
        }

    }

    private void OnEnable() {

        _waste.WasteUpdated += MoveHandler;
        _waste.WasteUpdated += Win;
        _editor.EditModeUpdated += EditModeHandler;

    }

    private void OnDisable() {

        _waste.WasteUpdated -= MoveHandler;
        _waste.WasteUpdated -= Win;
        _editor.EditModeUpdated -= EditModeHandler;

    }

    public void Undo() {

        if(InEditMode) return;

        if(_waste.GetLastCard() == null) return;

        _waste.GetLastCard().Undo();
        _waste.RemoveLastCard();

    }

    public void MoveHandler() {

        _score.Add(1);

        AudioSource.PlayOneShot(_cardSound);

    }

    public void EditModeHandler() {

        if(InEditMode == false && SelectedCard != null) {
            _editor.DeselectCard();
        }

    }

    private void Win() {

        if(_waste.GetPeakCardsCount() == _deck.GetInitialCardsInDeck().Count) {
            _winText.gameObject.SetActive(true);

            float animationSpeed = 1.5f;
            Sequence winTextAnimation = DOTween.Sequence()
                .Append(_winText.DOColor(Color.red, animationSpeed).SetEase(Ease.OutQuad))
                .Append(_winText.DOColor(Color.green, animationSpeed).SetEase(Ease.OutQuad))
                .Append(_winText.DOColor(Color.blue, animationSpeed).SetEase(Ease.OutQuad))
                .Append(_winText.DOColor(Color.magenta, animationSpeed).SetEase(Ease.OutQuad))
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);

            _waste.WasteUpdated -= Win;
        }

    }

    public Waste GetWaste() {

        return _waste;

    }

    public Deck GetDeck() {

        return _deck;

    }

    public DeckEditor GetDeckEditor() {

        return _editor;

    }

    public CardSettings GetRandomCardSettings() {

        if(_minimumRequiredCards.Count == 0) {
            _minimumRequiredCards.AddRange(_possibleCards);
        }

        int random = Random.Range(0, _minimumRequiredCards.Count);
        CardSettings settings = _minimumRequiredCards[random];
        _minimumRequiredCards.Remove(settings);

        return settings;

    }

    public CardSettings[] GetPossibleCards() {

        return _possibleCards;

    }

    public bool IsSimilar(ICard cardInWaste, ICard desiredCard) {

        sbyte rating1 = cardInWaste.GetRating();
        sbyte rating2 = desiredCard.GetRating();

        if((rating2 == 11 && rating1 == 10) || (rating2 == 10 && rating1 == 11)) return false;

        if(rating2 == rating1 + 1) return true;
        else if(rating2 == rating1 - 1) return true;
        else if((rating2 == 13 && rating1 == 10) || (rating2 == 10 && rating1 == 13)) return true;

        return false;

    }

    //В каждой пирамиде из карт, если считать два стоящих рядом ряда,
    //номера соседних снизу карт будут таковыми, что один номер
    //будет равен номеру карты верхнего ряда, а другой номер на единицу больше.
    public bool IsClosed(PeakCard card) {

        if(card.InWaste) return false;

        List<byte> peaks = card.Position.peak;
        byte row = 0;
        List<byte> orders = card.Position.order;

        if((card.Position.row - 1) < 0) return false;
        else row = (byte)(card.Position.row - 1);

        foreach(byte peak in peaks) {
            foreach(byte order in orders) {
                PeakCard leftCard = _deck.FindCardByPosition(peak, order, row);
                PeakCard rightCard = _deck.FindCardByPosition(peak, (byte)(order + 1), row);

                if(leftCard == null || rightCard == null) break;

                if(leftCard.InWaste && rightCard.InWaste) return false;
            }
        }

        return true;

    }

}
