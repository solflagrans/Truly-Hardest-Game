using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    //Задаёт передвижение для любого вида карт.
    //От любой к любой позиции.

    [field: SerializeField] public RectTransform RectTransform { get; private set; }

    TriPeaksManager _manager;
    DeckEditor _deckEditor;
    ICard _card;

    float _durationMultiplier = 0.02f;

    Vector2 _initialPosition;
    Vector3 _initialScale;
    Transform _initialParent;
    sbyte _initialChildCount;

    public bool IsDrag { get; private set; }

    private void OnValidate() {

        if(RectTransform == null) RectTransform = GetComponent<RectTransform>();

    }

    private void Awake() {

        TryGetComponent(out _card);

    }

    private void OnEnable() {

        if(_deckEditor != null) _deckEditor.EditModeUpdated += UpdateSelectedState;

    }

    private void OnDisable() {

        if(_deckEditor != null) _deckEditor.EditModeUpdated -= UpdateSelectedState;

    }

    private void Start() {

        _initialPosition = transform.position;
        _initialScale = transform.localScale;
        _initialParent = transform.parent;
        _initialChildCount = (sbyte) transform.GetSiblingIndex();

        _initialPosition = RectTransform.anchoredPosition;

        _manager = TriPeaksManager.Instance;
        _deckEditor = _manager.GetDeckEditor();

        _deckEditor.EditModeUpdated += UpdateSelectedState;

    }

    public void MoveToWastePosition() {

        Transform waste = _manager.GetWaste().transform;

        float duration = Mathf.Sqrt(Vector3.Distance(transform.position, waste.position)) * _durationMultiplier;
        transform.DOMove(waste.position, duration);

        transform.SetParent(waste.GetChild(0));
        transform.SetAsLastSibling();

    }

    public void UndoMovement() {

        float duration = Mathf.Sqrt(Vector3.Distance(transform.position, _initialPosition)) * _durationMultiplier;
        RectTransform.DOAnchorPos(_initialPosition, duration);

        transform.SetParent(_initialParent);
        transform.SetSiblingIndex(_initialChildCount);

    }

    public void UpdateSelectedState() {

        if(_manager.SelectedCard == _card) {
            transform.DOScale(_initialScale * 1.2f, _durationMultiplier * 20f);
        } else {
            transform.DOScale(_initialScale, _durationMultiplier * 25f);
        }

    }

    public void OnBeginDrag(PointerEventData eventData) {

        if(_card.InWaste || _card.IsClosed || _manager.InEditMode) return;

        IsDrag = true;

        _card.Image.raycastTarget = false;

    }

    public void OnDrag(PointerEventData eventData) {

        if(_card.InWaste || _card.IsClosed || _manager.InEditMode) {
            RectTransform.anchoredPosition = _initialPosition;
            return;
        }

        RectTransform.anchoredPosition += eventData.delta / _card.Image.canvas.scaleFactor;

    }

    public void OnEndDrag(PointerEventData eventData) {

        IsDrag = false;

        if(_card.InWaste || _card.IsClosed || _manager.InEditMode) return;

        _card.Image.raycastTarget = true;

        float duration = Mathf.Sqrt(Vector3.Distance(transform.position, _initialPosition)) * _durationMultiplier;
        RectTransform.DOAnchorPos(_initialPosition, duration);

    }

}
