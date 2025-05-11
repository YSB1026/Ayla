using UnityEngine;

public class CreditScroller : MonoBehaviour
{
    public float scrollSpeed = 50f; // 픽셀/초
    public float endY = 1200f;      // 목표 Y 위치

    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (rect.anchoredPosition.y < endY)
        {
            rect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }
}
