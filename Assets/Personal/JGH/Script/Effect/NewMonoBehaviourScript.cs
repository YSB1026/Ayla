using System.Collections;
using UnityEngine;

public class EffectDrop : MonoBehaviour
{
    IEnumerator GlowEffect(Light pointLight)
{
    float duration = 0.5f;
    float time = 0;
    while (time < duration)
    {
        float intensity = Mathf.Lerp(2f, 0f, time / duration);
        pointLight.intensity = intensity;
        time += Time.deltaTime;
        yield return null;
    }
    pointLight.enabled = false;
}

}
