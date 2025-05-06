using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenDarkener : MonoBehaviour
{
    private Image overlayImage;
    private Coroutine currentFade;
    private float currentTargetAlpha = -1f; // ���� ���� ���� ��ǥ ���İ�

    private void Awake()
    {
        overlayImage = GetComponent<Image>();
        if (overlayImage == null)
        {
            Debug.LogError("ScreenDarkener: Image ������Ʈ�� �����ϴ�.");
        }
    }

    // ���� 0 �� 1 ���� õõ�� ��ο���
    public void FadeToDark(float duration)
    {
        if (Mathf.Approximately(currentTargetAlpha, 1f))
            return; // �̹� ��ο����� ���̰ų� �Ϸ��

        currentTargetAlpha = 1f;
        StartFade(overlayImage.color.a, 1f, duration);

    }

    // ��� �ǵ��ư� (1 �� 0)
    public void FadeToClear(float duration)
    {
        if (Mathf.Approximately(currentTargetAlpha, 0f))
            return; // �̹� ������� ���̰ų� �Ϸ��

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
