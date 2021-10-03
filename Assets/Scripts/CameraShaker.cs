using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;

    // Screenshake
    private readonly float m_ScreenshakeMaxPosition = 2;
    private readonly float m_ScreenshakeMaxRotation = 10;

    private float m_ScreenshakeAmount;
    private Vector3 m_ScreenshakePosition;
    private Vector3 m_ScreenshakeRotation;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        UpdateScreenshake();
        transform.localPosition = m_ScreenshakePosition;
        transform.localEulerAngles = m_ScreenshakeRotation;
    }

    
    private void UpdateScreenshake()
    {
        m_ScreenshakePosition.x = (Mathf.PerlinNoise(Time.time * 4.5f, 3) * m_ScreenshakeMaxPosition) * m_ScreenshakeAmount;
        m_ScreenshakePosition.y = (Mathf.PerlinNoise(Time.time * 4.5f, 4) * m_ScreenshakeMaxPosition) * m_ScreenshakeAmount;
        m_ScreenshakePosition.z = (Mathf.PerlinNoise(Time.time * 4.5f, 5) * m_ScreenshakeMaxPosition) * m_ScreenshakeAmount;

        m_ScreenshakeRotation.x = (Mathf.PerlinNoise(Time.time * 3.5f, 6) * m_ScreenshakeMaxRotation) * m_ScreenshakeAmount;
        m_ScreenshakeRotation.y = (Mathf.PerlinNoise(Time.time * 3.5f, 7) * m_ScreenshakeMaxRotation) * m_ScreenshakeAmount;
        m_ScreenshakeRotation.z = (Mathf.PerlinNoise(Time.time * 3.5f, 8) * m_ScreenshakeMaxRotation) * m_ScreenshakeAmount;

        m_ScreenshakeAmount = Mathf.Lerp(m_ScreenshakeAmount, 0, 3 * Time.deltaTime);
    } 
    public void AddScreenshake(float amount)
    {
        m_ScreenshakeAmount += amount;
    }
}