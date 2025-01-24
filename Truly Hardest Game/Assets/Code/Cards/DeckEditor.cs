using System;
using UnityEngine;

public class DeckEditor : MonoBehaviour
{

    //Основной функционал для редактора карт

    [SerializeField] GameObject _editor;
    [SerializeField] GameObject _container;
    [SerializeField] GameObject _cardPrefab;
    [SerializeField] AudioClip _buttonSound;

    TriPeaksManager _manager;

    public event Action EditModeUpdated;

    private void Start() {
        
        _manager = TriPeaksManager.Instance;

        SpawnCards();

    }

    private void SpawnCards() {

        foreach(CardSettings settings in _manager.GetPossibleCards()) {
            EditorCard card = Instantiate(_cardPrefab, _container.transform).GetComponent<EditorCard>();
            card.Initialize(settings);
        }

    }

    public void SwitchEditMode() {

        _manager.InEditMode = !_manager.InEditMode;

        _manager.AudioSource.PlayOneShot(_buttonSound);

        if(_manager.SelectedCard != null) DeselectCard();

        _editor.SetActive(_manager.InEditMode);

        EditModeUpdated?.Invoke();

    }

    public void SelectCard(ICard card) {

        if(_manager.SelectedCard != null) DeselectCard();

        _manager.SelectedCard = card;

        EditModeUpdated?.Invoke();

    }

    public void DeselectCard() {

        _manager.SelectedCard = null;

    }

    public void ChangeCard(CardSettings settings) {

        if(_manager.SelectedCard == null) return;

        _manager.SelectedCard.Settings = settings;
        _manager.SelectedCard.Initialize();

    }

}
