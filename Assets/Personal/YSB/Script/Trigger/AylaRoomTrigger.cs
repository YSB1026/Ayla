using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AylaRoomEvent : MonoBehaviour
{
    [SerializeField] private GameObject aylaRoomBeforeCutScene;
    [SerializeField] private GameObject aylaRoomAfterCutScene;

    [SerializeField] private float fadeDuration = 4f;

    private void OnEnable()
    {
        StartCoroutine(AfterGreenPendantTrigger());
    }

    IEnumerator AfterGreenPendantTrigger()
    {
        Debug.Log("green event 호출됐어요!");
        yield return null;

        Coroutine fadeOut = StartCoroutine(FadeSpritesOutAndDisable(aylaRoomBeforeCutScene));
        Coroutine fadeIn = StartCoroutine(FadeSpritesIn(aylaRoomAfterCutScene));

        yield return fadeOut;
        yield return fadeIn;

        SelftDestroy();
    }

    IEnumerator FadeSpritesOutAndDisable(GameObject parent)
    {
        SpriteRenderer[] sprites = parent.GetComponentsInChildren<SpriteRenderer>();
        float time = 0f;

        Dictionary<SpriteRenderer, Color> originalColors = new();

        foreach (var sprite in sprites)
        {
            originalColors[sprite] = sprite.color;
        }

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            foreach (var sprite in sprites)
            {
                if (sprite != null)
                {
                    Color c = originalColors[sprite];
                    c.a = alpha;
                    sprite.color = c;
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        // 완전히 투명하게 만들고 오브젝트 비활성화
        foreach (var sprite in sprites)
        {
            if (sprite != null)
            {
                Color c = originalColors[sprite];
                c.a = 0f;
                sprite.color = c;
            }
        }

        parent.SetActive(false);
    }

    IEnumerator FadeSpritesIn(GameObject parent)
    {
        parent.SetActive(true);

        SpriteRenderer[] sprites = parent.GetComponentsInChildren<SpriteRenderer>();
        float time = 0f;

        Dictionary<SpriteRenderer, Color> targetColors = new();

        foreach (var sprite in sprites)
        {
            Color c = sprite.color;
            c.a = 0f;
            sprite.color = c; // 처음엔 투명하게
            targetColors[sprite] = c;
        }

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            foreach (var sprite in sprites)
            {
                if (sprite != null)
                {
                    Color c = targetColors[sprite];
                    c.a = alpha;
                    sprite.color = c;
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        // 완전 불투명으로 보정
        foreach (var sprite in sprites)
        {
            if (sprite != null)
            {
                Color c = targetColors[sprite];
                c.a = 1f;
                sprite.color = c;
            }
        }
    }

    private void SelftDestroy()
    {
        Destroy(gameObject);
    }
}
