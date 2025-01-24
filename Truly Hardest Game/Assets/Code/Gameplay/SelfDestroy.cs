using UnityEngine;

public class SelfDestroy : MonoBehaviour
{

    [Tooltip("Set -1 to disable AutoDestroy")]
    [SerializeField] float _timeBeforeDestroy;
    [SerializeField] bool _softDestroy;

    float _timer;

    private void Update() {

        if(_timeBeforeDestroy == -1) return;

        if(_timer < _timeBeforeDestroy) {
            _timer += Time.deltaTime;
        } else {
            Destroy();
        }

    }

    public void Destroy() {

        if(_softDestroy) gameObject.SetActive(false);
        else Destroy(gameObject);

    }

}
