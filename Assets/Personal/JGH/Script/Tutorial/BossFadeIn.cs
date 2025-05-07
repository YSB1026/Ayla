using UnityEngine;

public class BossFadeIn : MonoBehaviour
{
    public float fadeDuration = 2f; // 몇 초 동안 페이드인할지
    private SpriteRenderer spriteRenderer;
    private float timer = 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 처음엔 완전히 투명
        Color c = spriteRenderer.color;
        c.a = 0f;
        spriteRenderer.color = c;
    }

    private void OnEnable()
    {
        // 페이드인 시작
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;

            yield return null;
        }
    }
}
