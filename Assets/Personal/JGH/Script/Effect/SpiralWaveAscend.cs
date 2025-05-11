using UnityEngine;

public class SpiralWaveAscend_PhaseOffset : MonoBehaviour
{
    public Transform[] spheres;          // 3개 구체
    public float waveAmplitude = 1f;     // 진폭 (좌우 흔들림)
    public float waveFrequency = 2f;     // 파형 속도
    public float riseSpeed = 1f;         // Y축 상승 속도

    private float[] phaseOffsets;

    void Start()
    {
        // 위상 차이를 120도 간격으로 분산 (2파이 기준)
        phaseOffsets = new float[spheres.Length];
        for (int i = 0; i < spheres.Length; i++)
        {
            phaseOffsets[i] = Mathf.PI * 2f / spheres.Length * i;
        }
    }

    void Update()
    {
        float t = Time.time;

        for (int i = 0; i < spheres.Length; i++)
        {
            float y = t * riseSpeed;
            float x = Mathf.Sin(t * waveFrequency + phaseOffsets[i]) * waveAmplitude;

            spheres[i].position = new Vector3(x, y, 0f);
        }
    }
}
