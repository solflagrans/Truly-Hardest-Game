using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PeakCard : MonoBehaviour, IPointerClickHandler, ICard {

    //Карты, которые находятся в основной колоде и являются
    //необходимым для победы.

    [field: SerializeField] public Image Image { get; set; }
    [SerializeField] CardMovement _cardMovement;

    TriPeaksManager _manager;
    Waste _waste;
    DeckEditor _deckEditor;

    public CardSettings Settings { get; set; }
    public CardPosition Position;

    public bool InWaste { get; set; }
    public bool IsClosed { get; set; }

    public struct CardPosition {
        public List<byte> peak;
        public List<byte> order;
        public byte row;
    }

    private void OnValidate() {

        if(Image == null) Image = GetComponent<Image>();
        if(_cardMovement == null) TryGetComponent(out _cardMovement);

    }

    private void OnEnable() {

        if(_waste != null) _waste.WasteUpdated += UpdateState;
        if(_deckEditor != null) _deckEditor.EditModeUpdated += UpdateState;

    }

    private void OnDisable() {

        if(_waste != null) _waste.WasteUpdated -= UpdateState;
        if(_deckEditor != null) _deckEditor.EditModeUpdated -= UpdateState;

    }

    private void Start() {

        _manager = TriPeaksManager.Instance;
        _waste = _manager.GetWaste();
        _deckEditor = _manager.GetDeckEditor();

        Initialize();

        _waste.WasteUpdated += UpdateState;
        _deckEditor.EditModeUpdated += UpdateState;
        UpdateState();

    }

    public void Initialize() {

        if(Settings == null) Settings = _manager.GetRandomCardSettings();
        Image.sprite = Settings._image;

    }

    public void InitializeDeckPosition(byte peak, byte order, byte row) {

        if(Position.peak == null) Position.peak = new List<byte>();
        if(Position.order == null) Position.order = new List<byte>();

        Position.peak.Add(peak);
        Position.order.Add(order);
        Position.row = row;

    }

    public void OnPointerClick(PointerEventData eventData) {

        if(!_cardMovement.IsDrag && !_manager.InEditMode && !IsClosed && !InWaste) {
            MoveToWaste();
        }

        if(_manager.InEditMode) {
            if((PeakCard) _manager.SelectedCard == this) return;
            _deckEditor.SelectCard(this);
        }

    }

    public void MoveToWaste() {

        if(InWaste || _manager.InEditMode) return;

        ICard lastCard = _waste.GetLastCard();

        if(lastCard == null || _manager.IsSimilar(lastCard, this)) {
            InWaste = true;

            _waste.AddCard(this);

            _cardMovement.MoveToWastePosition();

            Image.raycastTarget = false;
        }

    }

    public void Undo() {

        _cardMovement.UndoMovement();

        Image.raycastTarget = true;

        InWaste = false;

    }

    public void UpdateState(bool closed) {

        IsClosed = closed;

        if(IsClosed) {
            Image.sprite = _manager.ClosedCardSprite;
        } else {
            Image.sprite = Settings._image;
        }

    }

    public void UpdateState() {

        if(_manager.InEditMode) {
            IsClosed = false;
        } else {
            IsClosed = _manager.IsClosed(this);
        }

        UpdateState(IsClosed);

    }

    public sbyte GetRating() {

        return Settings._rating;

    }

}
