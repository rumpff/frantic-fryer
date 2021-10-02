using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public const float ClipBPM = 150;

    public float ElapsedTime { get; private set; }

    public float ElapsedBeats
    {
        get
        {
            return ElapsedTime / 60.0f * ClipBPM;
        }
    }
    private int _loopAmount;

    [SerializeField] private AudioClip _defaultClip;
    [SerializeField] private TextMeshPro test;
    private AudioSource _audioSource; // Audiosource for music clips

    private float _previousTime;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _previousTime = 0;
        ElapsedTime = 0;
        _loopAmount = 0;

        _audioSource.clip = _defaultClip;
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = _audioSource.time;

        if (_previousTime > currentTime)
        {
            // has looped
            _loopAmount++;
        }

        ElapsedTime = currentTime + (_loopAmount * _defaultClip.length);

        _previousTime = currentTime;

        test.text = $"time: {ElapsedTime:##.000}\n" +
                    $"beat: {ElapsedBeats:##.000}";
    }
}
