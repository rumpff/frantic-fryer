using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public readonly float ClipBPM = 150;

    public float ElapsedTime { get; set; }
    public int LoopAmount { get; set; }

    public float ElapsedBeats
    {
        get
        {
            return ElapsedTime / 60.0f * ClipBPM;
        }
    }

    [SerializeField] private AudioClip _defaultClip;
    [SerializeField] private TextMeshPro test;
    public AudioSource _audioSource; // Audiosource for music clips

    private float _previousTime;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();

        _previousTime = 0;
        ElapsedTime = 0;
        LoopAmount = 0;

        _audioSource.clip = _defaultClip;
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = _audioSource.time;

        if (_previousTime > currentTime)
        {
            OnLoop();
        }

        ElapsedTime = currentTime + (LoopAmount * (60.0f / ClipBPM * GameManager.patternLength));

        _previousTime = currentTime;

        test.text = $"loop: {LoopAmount}\n" +
                    $"time: {ElapsedTime:##.000}\n" +
                    $"beat: {ElapsedBeats:##.000}";
    }

    public void PlayClip(AudioClip newClip)
    {
        _audioSource.Stop();
        _audioSource.clip = newClip;
        _audioSource.Play();

        _previousTime = -1;
    }

    private void OnLoop()
    {
        LoopAmount++;
        GameManager.Instance.NextPattern();
    }
}
