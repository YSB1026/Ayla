using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public bool isPuzzleComplete = false;

    public void CompletePuzzle()
    {
        if (isPuzzleComplete) return;

        isPuzzleComplete = true;
        GreenPendantEvent.OnGreenPuzzleSolved?.Invoke();
    }
}
