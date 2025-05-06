using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenDarkener : MonoBehaviour
{
    private Image overlayImage;
    private Coroutine currentFade;
    private float currentTargetAlpha = -1f; // 현재 진행 중인 목표 알파값

    private void Awake()
    {
        overlayImage = GetComponent<Image>();
        if (overlayImage == null)
        {
            Debug.LogError("ScreenDarkener: Image 컴포넌트가 없습니다.");
        }
    }

    // 알파 0 → 1 까지 천천히 어두워짐
    public void FadeToDark(float duration)
    {
        if (Mathf.Approximately(currentTargetAlpha, 1f))
            return; // 이미 어두워지는 중이거나 완료됨

        currentTargetAlpha = 1f;
        StartFade(overlayImage.color.a, 1f, duration);

    }

    // 밝게 되돌아감 (1 → 0)
    public void FadeToClear(float duration)
    {
        if (Mathf.Approximately(currentTargetAlpha, 0f))
            return; // 이미 밝아지는 중이거나 완료됨

        currentTargetAlpha = 0f;
        StartFade(overlayImage.color.a, 0f, duration);
    }

    private void StartFade(float from, float to, float duration)
    {
        if (currentFade != null)
            StopCoroutine(currentFade);
        currentFade = StartCoroutine(Fade(from, to, duration));
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;                   // y = x
            float alpha = from + (to - from) * t;

            overlayImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        overlayImage.color = new Color(0, 0, 0, to);
        currentFade = null;
        currentTargetAlpha = to;
    }
}
