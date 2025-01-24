using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorCard : MonoBehaviour, IPointerClickHandler {

    //Этот класс никак не относится к интерфейсу ICard
    //Используется, как пункт выбора в редакторе

    [SerializeField] Image _image;
    
    TriPeaksManager _manager;
    CardSettings _settings;

    private void Start() {
        
        _manager = TriPeaksManager.Instance;

    }

    public void Initialize(CardSettings settings) {

        _settings = settings;
        _image.sprite = settings._image;

    }

    public void OnPointerClick(PointerEventData eventData) {

        _manager.GetDeckEditor().ChangeCard(_settings);

    }

    public CardSettings GetSettings() {

        return _settings;

    }
}
