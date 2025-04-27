using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public bool InLight = false;
    public Transform targetChild; // 🔥 사이즈 변경할 자식 오브젝트

    private Vector3 originalScale;
    public Vector3 lightScale = new Vector3(1.5f, 1.5f, 1.5f);
    public float transitionDuration = 0.5f;

    private Vector3 targetScale;
    private float transitionTimer = 0f;

    void Start()
    {
        if (targetChild == null)
        {
            Debug.LogError("PlayerState: targetChild를 설정해주세요!");
            enabled = false;
            return;
        }

        originalScale = targetChild.localScale; // 자식 오브젝트의 원래 크기 저장
        targetScale = originalScale;
    }

    void Update()
    {
        if (InLight && targetScale != lightScale)
        {
            targetScale = lightScale;
            transitionTimer = 0f;
        }
        else if (!InLight && targetScale != originalScale)
        {
            targetScale = originalScale;
            transitionTimer = 0f;
        }

        if (transitionTimer < transitionDuration)
        {
            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / transitionDuration);
            targetChild.localScale = Vector3.Lerp(targetChild.localScale, targetScale, t);
        }
    }
}
