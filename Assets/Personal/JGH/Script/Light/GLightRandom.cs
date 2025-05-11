using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal; // Light2D를 쓰기 위해 필요

public class GLightRandom : MonoBehaviour
{
     public Light2D light2D;
    public float minIntensity = 0.1f;
    public float maxIntensity = 0.5f;
    public float minDelay = 0.05f;
    public float maxDelay = 0.2f;

    private void Start()
    {
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            light2D.intensity = Random.Range(minIntensity, maxIntensity);
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }
}
