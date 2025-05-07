using UnityEngine;
using UnityEngine.UI;

public class TextColorBlink : MonoBehaviour
{
    public Color colorA = Color.white;
    public Color colorB = Color.red;
    public float blinkSpeed = 1.0f;

    private Text text;
    private float t;

    void Start()
    {
        text = GetComponent<Text>();
        if (text == null)
        {
            Debug.LogWarning("SmoothTextBlink: Text ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    void Update()
    {
        if (text == null) return;

        t += Time.deltaTime * blinkSpeed;
        text.color = Color.Lerp(colorA, colorB, Mathf.PingPong(t, 1));
    }
}
