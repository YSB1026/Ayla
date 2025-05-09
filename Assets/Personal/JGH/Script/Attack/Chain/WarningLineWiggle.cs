using UnityEngine;

public class WarningLineWiggle : MonoBehaviour
{
    public float wiggleAmount = 0.1f;
    public float wiggleSpeed = 10f;
    private Vector3 originalPos;

    void Start() => originalPos = transform.position;

    void Update()
    {
        float offsetX = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
        transform.position = originalPos + new Vector3(offsetX, 0, 0);
    }
}
