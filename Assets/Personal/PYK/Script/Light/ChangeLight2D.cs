using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChangeLight2D : MonoBehaviour
{
    [Header("Intensity Settings")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.0f;
    public float cycleDuration = 5f; // 한 사이클에 걸리는 시간 (초)

    private Light2D light2D;
    private float timer;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        float normalizedTime = (timer % cycleDuration) / cycleDuration; // 0 ~ 1 사이값
        float curve = Mathf.Sin(normalizedTime * Mathf.PI * 2) * 0.5f + 0.5f; // 0 ~ 1 사인 곡선

        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, curve);
    }
}
