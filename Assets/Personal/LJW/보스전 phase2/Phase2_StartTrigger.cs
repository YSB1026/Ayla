/*using UnityEngine;

public class Phase2_StartTrigger : MonoBehaviour
{
    public Phase2Controller phase2Controller;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;
            phase2Controller.StartPhase2();
            Debug.Log("[Phase2] Start Trigger");

            gameObject.SetActive(false);
        }
    }
}
*/