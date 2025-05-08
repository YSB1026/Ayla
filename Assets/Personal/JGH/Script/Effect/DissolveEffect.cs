using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    public Material targetMaterial; // 머터리얼 할당
    public float duration = 2.0f;   // 1 → 0 으로 줄이는 시간

    private void Start()
    {
        StartCoroutine(FadeDissolveAmount());
    }

    private System.Collections.IEnumerator FadeDissolveAmount()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("Target material is not assigned.");
            yield break;
        }

        // DissolveAmount 값 확인
        float currentValue = targetMaterial.GetFloat("_DissolveAmount");

        // 1이 아니라면 먼저 1로 설정
        if (Mathf.Abs(currentValue - 1f) > 0.001f)
        {
            targetMaterial.SetFloat("_DissolveAmount", 1f);
            yield return new WaitForSeconds(0.1f); // 약간 대기 후 진행
        }

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float newValue = Mathf.Lerp(1f, 0f, time / duration);
            targetMaterial.SetFloat("_DissolveAmount", newValue);
            yield return null;
        }

        targetMaterial.SetFloat("_DissolveAmount", 0f);
    }
}
