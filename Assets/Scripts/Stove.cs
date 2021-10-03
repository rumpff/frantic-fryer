using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class Stove : MonoBehaviour
{
    private readonly Color _progressNormal = Color.green;
    private readonly Color _progressTension = Color.red;

    private bool _isOccupied;
    private bool _isFrying;

    private Vector2 _baseBarPos;

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
            _progressCanvas.gameObject.SetActive(value);
            _isFrying = value;
        }
    }

    [SerializeField] private Canvas _progressCanvas;
    [SerializeField] private Image _progressBar;

    // Start is called before the first frame update
    void Start()
    {
        _baseBarPos = _progressBar.rectTransform.anchoredPosition;
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
}
