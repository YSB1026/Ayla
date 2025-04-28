using System.Collections;
using UnityEngine;

public class TextDissolve : MonoBehaviour
{
    public Renderer renderer;
    public ParticleSystem dissolveParticles; // 파티클 시스템 추가
    public float duration = 1.5f;

    private string dissolveAmount = "_DissolveAmount"; 

    void Start()
    {
        if (dissolveParticles != null)
        {
            dissolveParticles.Play(); // 파티클 시작
        }
        StartCoroutine(TextDissolveDuration());
    }

    public IEnumerator TextDissolveDuration()
    {
        Material mat = renderer.material;
        float elapsed = 0f;
        float start = 0f;
        float end = 1f;

        while (elapsed < duration)
        {
            float value = Mathf.Lerp(start, end, elapsed / duration);
            mat.SetFloat(dissolveAmount, value);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mat.SetFloat(dissolveAmount, end);
    }
}
