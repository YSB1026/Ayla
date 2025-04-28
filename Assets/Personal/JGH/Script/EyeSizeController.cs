using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EyeSizeController : MonoBehaviour
{
    [System.Serializable]
    public class Eye
    {
        public Renderer renderer;
        public float maxSize = 0.5f; // 마지막 눈은 1.0으로 설정
    }

    public List<Eye> eyes; // 순서대로 5개 넣기
    public float duration = 1.5f;
    private string sizeProperty = "_Size"; // Shader Graph 내부에서 확인 필요

    void Start()
    {
        for (int i = 0; i < eyes.Count; i++)
        {
            var instMat = new Material(eyes[i].renderer.sharedMaterial);
            eyes[i].renderer.material = instMat;
        }

        StartCoroutine(AnimateOnce());
    }

    IEnumerator AnimateOnce()
    {
        foreach (var eye in eyes)
        {
            Material mat = eye.renderer.material;
            float elapsed = 0f;
            float start = 0f;
            float end = eye.maxSize;

            while (elapsed < duration)
            {
                float value = Mathf.Lerp(start, end, elapsed / duration);
                mat.SetFloat(sizeProperty, value);
                elapsed += Time.deltaTime;
                yield return null;
            }

            mat.SetFloat(sizeProperty, end);
        }
    }
}
