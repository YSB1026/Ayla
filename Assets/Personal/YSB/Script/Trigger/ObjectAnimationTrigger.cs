using UnityEngine;

public class ObjectAnimationTrigger : MonoBehaviour
{
    [Header("Object that must enter the trigger")]
    [SerializeField] private GameObject triggerTarget; 

    [Header("Object to animate")]
    [SerializeField] private GameObject animatedObject; 

    [Header("Object to activate (Can be null)")]
    [SerializeField] private GameObject objectToActivate;

    [Header("Destroy Object")]
    [SerializeField] private bool isDestroy;

    private void OnValidate()
    {
        if (triggerTarget == null)
        {
            Debug.LogWarning($"[{name}] Trigger target is not assigned in ObjectAnimationTrigger.");
        }

        if (animatedObject == null)
        {
            Debug.LogWarning($"[{name}] Animated object is not assigned in ObjectAnimationTrigger.");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == triggerTarget)
        {
            animatedObject.GetComponent<Animator>().SetBool("isOpen", true);

            if (objectToActivate != null) objectToActivate.SetActive(true);
            if (isDestroy) Destroy(gameObject);
        }
    }
}
