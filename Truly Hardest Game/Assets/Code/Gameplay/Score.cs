using System;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _scoreText;

    [SerializeField] uint _initialScore;
    [SerializeField] string _prefix;
    [SerializeField] string _suffix;

    public event Action ScoreUpdated; 

    uint _points;

    private void Awake() {
        
        Set(_initialScore);
        UpdateScore();

    }

    public void Add(uint points) {

        _points += points;
        UpdateScore();

    }

    public void Subtract(uint points) {

        _points -= points;
        UpdateScore();

    }

    public void Set(uint points) {

        _points = points;
        UpdateScore();

    }

    public void Clear(uint points) {

        _points = 0;
        UpdateScore();

    }

    private void UpdateScore() {

        if(_scoreText != null) _scoreText.text = _prefix + _points + _suffix;
        ScoreUpdated?.Invoke();

    }

}
