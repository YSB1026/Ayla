using UnityEngine;
using UnityEngine.UI;

public class ColorBlink : MonoBehaviour
{
    public Color colorA = Color.clear;
    public Color colorB = new Color(1, 0, 0, 0.5f); // π›≈ı∏Ì ª°∞≠
    public float blinkSpeed = 1.5f;

    private Image image;
    private float t;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        t += Time.deltaTime * blinkSpeed;
        image.color = Color.Lerp(colorA, colorB, Mathf.PingPong(t, 1));
    }
}
