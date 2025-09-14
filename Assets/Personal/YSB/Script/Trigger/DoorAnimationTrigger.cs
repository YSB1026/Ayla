using UnityEngine;

// public enum ObjectAnimationType
// {
//     Default,
//     Open,
//     Broken
// }
public class DoorAnimationTrigger : BaseTrigger
{
    [Header("오브젝트 애니메이션 타입")]
    //[SerializeField] private ObjectAnimationType animationType;
    // [SerializeField] private ILightReactive lightReactive;
    // public ILightReactive LightReactive => lightReactive;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnPlayerEnter()
    {
        animator.SetBool("isOpen", true);
    }

    // public void ApplyAnimation()
    // {
    //     switch (animationType)
    //     {
    //         case ObjectAnimationType.Open:
    //             animator.SetBool("isOpen", true);
    //             break;
    //         case ObjectAnimationType.Broken:
    //             animator.SetBool("isBroken", true);
    //             break;
    //         default:
    //             break;
    //     }
    // }

}
