using UnityEngine;

public abstract class BaseTrigger : MonoBehaviour
{
    protected abstract void OnPlayerEnter();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPlayerEnter();
        }
    }
}
