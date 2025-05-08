using UnityEngine;

public class Phase1_PuzzleTrigger : MonoBehaviour
{
    private bool solved = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (solved) return;

        if (collision.CompareTag("Ayla"))
        {
            FindAnyObjectByType<Phase1_Manager>()?.SolvePuzzle();
            solved = true;
        }
    }
}
