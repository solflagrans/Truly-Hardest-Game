using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _timer;

    float _timeElapsed;

    [field: SerializeField] bool Paused { get; set; }

    private void FixedUpdate() {

        if(!Paused) CountTime();

    }

    private void CountTime() {

        _timeElapsed += Time.deltaTime;

        TimeSpan time = TimeSpan.FromSeconds(_timeElapsed);
        if(time.Hours == 0) {
            _timer.text = time.ToString(@"mm\:ss");
        } else {
            _timer.text = time.ToString(@"hh\mm\:ss");
        }

    }

}
