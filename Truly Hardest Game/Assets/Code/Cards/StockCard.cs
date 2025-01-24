using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StockCard : MonoBehaviour, IPointerClickHandler, ICard {

    // арты, которые наход€тс€ в стопке карт и которые
    //можно брать в любой момент.

    [field: SerializeField] public Image Image { get; set; }
    [SerializeField] CardMovement _cardMovement;

    TriPeaksManager _manager;
    Waste _waste;

    public CardSettings Settings { get; set; }

    public bool InWaste { get; set; }
    public bool IsClosed { get; set; }

    private void OnValidate() {

        if(Image == null) Image = GetComponent<Image>();
        if(_cardMovement == null) TryGetComponent(out _cardMovement);

    }

    private void Start() {

        _manager = TriPeaksManager.Instance;
        _waste = _manager.GetWaste();

        Initialize();

        IsClosed = true;
        UpdateState(true);

    }

    public void Initialize() {

        if(Settings == null) Settings = _manager.GetRandomCardSettings();
        Image.sprite = Settings._image;

    }

    public void OnPointerClick(PointerEventData pointerEventData) {

        MoveToWaste();

    } 

    public void MoveToWaste() {

        if(InWaste) return;

        _waste.AddCard(this);
        _cardMovement.MoveToWastePosition();

        Image.raycastTarget = false;

        UpdateState(false);

        InWaste = true;

    }

    public void Undo() {

        _cardMovement.UndoMovement();

        Image.raycastTarget = true;

        IsClosed = true;

        UpdateState(true);

        InWaste = false;

    }

    public void UpdateState(bool closed) {

        IsClosed = closed;

        if(closed) {
            Image.sprite = _manager.ClosedCardSprite;
        } else {
            Image.sprite = Settings._image;
        }

    }

    public sbyte GetRating() {

        return Settings._rating;

    }

}