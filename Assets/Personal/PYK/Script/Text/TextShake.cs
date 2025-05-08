using UnityEngine;

public class TextShake : MonoBehaviour
{
    public float shakeAmount = 5f;
    public float shakeSpeed = 20f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        float offsetY = Mathf.Cos(Time.time * shakeSpeed) * shakeAmount;

        transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
    }
}
