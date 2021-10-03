using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class Stove : MonoBehaviour
{
    private readonly Color _progressNormal = Color.yellow;
    private readonly Color _progressTension = Color.green;

    private bool _isOccupied;
    private bool _isFrying;

    private Vector2 _baseBarPos;

    [SerializeField] private AudioClip _frySuccsesClip, _fryFailedClip;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private Canvas _progressCanvas;
    [SerializeField] private Image _progressBar;

    [Space(25)] [SerializeField] private Image _resultImage;
    [SerializeField] private Sprite _iconSucces, _iconFailed;
    [SerializeField] private Vector2 _iconEndPos;
    [SerializeField] private Vector3 _iconEndRotate;
    [SerializeField] private float _iconDuration;
    [SerializeField] private AnimationCurve _iconPosCurve, _iconRotCurve, _iconFadeCurve;

    public bool IsOccupied
    {
        get { return _isOccupied; }
        set { _isOccupied = value; }
    }

    public bool IsFrying
    {
        get { return _isFrying; }
        set
        {
            _progressBar.transform.parent.gameObject.SetActive(value);
            _isFrying = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _baseBarPos = _progressBar.rectTransform.anchoredPosition;
        _resultImage.gameObject.SetActive(false);

        Empty();
    }

    public void Empty()
    {
        IsOccupied = false;
        IsFrying = false;
    }

    public void SetProgressbar(float progress)
    {
        _progressBar.fillAmount = progress;
        if (progress < 1)
        {
            _progressBar.color = _progressNormal;
            _progressBar.rectTransform.anchoredPosition = _baseBarPos;
        }
        else
        {
            _progressBar.color = _progressTension;
            _progressBar.rectTransform.anchoredPosition = _baseBarPos + new Vector2()
            {
                x = Random.Range(-0.05f, 0.05f),
                y = Random.Range(-0.05f, 0.05f)
            };
        }
    }

    public void FrySucces()
    {
        _audioSource.PlayOneShot(_frySuccsesClip);
        StartCoroutine(IconAnimation(_iconSucces));
    }

    public void FryFailed()
    {
        StartCoroutine(IconAnimation(_iconFailed));
    }

    private IEnumerator IconAnimation(Sprite icon)
    {
        float t = 0;
        _resultImage.gameObject.SetActive(true);
        _resultImage.sprite = icon;

        while (t < 1)
        {
            t += Time.deltaTime / _iconDuration;

            _resultImage.rectTransform.anchoredPosition = _iconEndPos * _iconPosCurve.Evaluate(t);
            _resultImage.rectTransform.localEulerAngles = _iconEndRotate * _iconRotCurve.Evaluate(t);
            _resultImage.color = new Color(1, 1, 1, _iconFadeCurve.Evaluate(t));


            yield return new WaitForEndOfFrame();
        }

        _resultImage.gameObject.SetActive(false);
    }
}
