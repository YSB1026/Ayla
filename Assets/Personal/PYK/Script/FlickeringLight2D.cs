using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class FlickeringLight2D : MonoBehaviour
{
    public float minIntensity = 0.3f;
    public float maxIntensity = 1.5f;

    public float minFlickerTime = 0.05f;
    public float maxFlickerTime = 0.2f;

    private Light2D light2D;
    private Coroutine flickerRoutine;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    void OnEnable()
    {
        flickerRoutine = StartCoroutine(Flicker());
    }

    void OnDisable()
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            float newIntensity = Random.Range(minIntensity, maxIntensity);
            light2D.intensity = newIntensity;

            float waitTime = Random.Range(minFlickerTime, maxFlickerTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
