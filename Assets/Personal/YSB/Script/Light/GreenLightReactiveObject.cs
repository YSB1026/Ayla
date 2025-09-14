using UnityEngine;

public class GreenLightReactiveObject : MonoBehaviour, ILightReactive
{
    private Animator animator;
    private BoxCollider2D col;
    public bool IsInLight { get; set; } = false;
    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        ApplyLightReaction();
    }
    public void ApplyLightReaction()
    {
        if (IsInLight)
        {
            animator.SetBool("isBroken", false);
            col.enabled = false;
        }
        else
        {
            animator.SetBool("isBroken", true);
            col.enabled = true;
        }
    }
}
