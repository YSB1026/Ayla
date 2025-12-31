using UnityEngine;

public class GreenLightReactiveObject : MonoBehaviour, ILightReactive
{
    private bool isFirst = true;
    private Animator animator;
    private BoxCollider2D col;
    private bool isPermanentlyRestored = false; // 영구 복원 플래그 추가
    
    public bool IsInLight { get; set; } = false;
    
    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        ApplyLightReaction();
    }
    
    public void ApplyLightReaction()
    {
        if (!isFirst && GameManager.Instance.currentSave.pendantColor != ColorOption.Green) return;
        if (isFirst) isFirst = false;

        // 이미 영구 복원되었으면 더 이상 변경하지 않음
        if (isPermanentlyRestored) return;

        if (IsInLight)
        {
            Debug.Log("Green Light On - Permanently Restored");
            animator.SetBool("isBroken", false);
            col.enabled = false;
            isPermanentlyRestored = true; // 한번 복원되면 영구적으로 유지
        }
        else
        {
            animator.SetBool("isBroken", true);
            col.enabled = true;
        }
    }
}