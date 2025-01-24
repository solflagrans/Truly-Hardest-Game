using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.AddressableAssets;

public class BananaClicker : MonoBehaviour
{

    [Header("Instances")]
    [SerializeField] Canvas _canvas;
    [SerializeField] Score _score;
    [SerializeField] AssetReference _point;
    [SerializeField] Transform _pointSpawn;
    [SerializeField] AudioSource _audioSource;

    [Header("Settings")]
    [SerializeField] float _spinDuration;
    [SerializeField] float _pointDuration;
    [SerializeField] AudioClip _bananaClickSound; 

    Vector3 _initialRotation;

    Tween _rotationTween;

    bool _awatingAnimation;

    private void Start() {

        _rotationTween = transform
                .DOLocalRotate(new Vector3(0f, 0f, 360f), _spinDuration, RotateMode.FastBeyond360)
                .SetRelative()
                .SetEase(Ease.Linear)
                .SetAutoKill(false)
                .Pause();

    }

    public void Click() {

        _audioSource.PlayOneShot(_bananaClickSound);

        if(!_awatingAnimation) AnimateBanana().Forget();

        SpawnPoint().Forget();

        _score.Add(1);

    }

    async UniTaskVoid SpawnPoint() {

        float xDeviation = Random.Range(-200f, 200f) * _canvas.scaleFactor;
        Vector2 pointSpawn = new Vector2(_pointSpawn.position.x + xDeviation, _pointSpawn.position.y);

        GameObject point = await _point.InstantiateAsync(pointSpawn, Quaternion.identity, transform.parent);
        point.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);

        TextMeshProUGUI pointText = point.GetComponent<TextMeshProUGUI>();
        Color desiredColor = new Color(pointText.color.r, pointText.color.g, pointText.color.b, 0f);

        Sequence popOutPoint = DOTween.Sequence()
            .Append(point.transform.DOMove(point.transform.up * 250f * pointText.canvas.scaleFactor, _pointDuration * 0.6f)
            .SetEase(Ease.OutQuad)
            .SetRelative(true))
            .Append(pointText.DOColor(desiredColor, _pointDuration * 0.4f)
            .SetEase(Ease.Linear))
            .SetEase(Ease.Linear);

    }

    async UniTaskVoid AnimateBanana() {

        if(_rotationTween.IsPlaying()) {
            _awatingAnimation = true;
            await _rotationTween.AsyncWaitForCompletion();
            _rotationTween.Restart();
            _awatingAnimation = false;
        } else {
            _rotationTween.Restart();
        }

    }

}
