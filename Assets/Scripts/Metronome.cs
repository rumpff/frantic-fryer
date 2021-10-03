using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    [SerializeField] private Vector3 _swingAngle;
    [SerializeField] private AudioSource _tickAudioSource;
    [SerializeField] private Transform _swing;

    private int prevBeat;

    void Start()
    {
        prevBeat = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedBeats = MusicManager.Instance.ElapsedBeats;
        int currentBeat = Mathf.FloorToInt(elapsedBeats);

        if (currentBeat != prevBeat)
        {
            _tickAudioSource.Stop();
            _tickAudioSource.Play();
        }

        prevBeat = currentBeat;

        _swing.localEulerAngles = new Vector3()
        {
            x = _swingAngle.x * Mathf.Sin(elapsedBeats * Mathf.PI),
            y = _swingAngle.y * Mathf.Sin(elapsedBeats * Mathf.PI),
            z = _swingAngle.z * Mathf.Sin(elapsedBeats * Mathf.PI)
        };
    }
}
