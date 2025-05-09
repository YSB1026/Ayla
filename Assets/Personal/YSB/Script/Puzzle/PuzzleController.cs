//using System.Collections;
//using UnityEngine;

//public class PuzzleController : MonoBehaviour
//{
//    public bool isPuzzleComplete = false;
//    public void CompletePuzzle()
//    {
//        if (isPuzzleComplete) return;

//        StartCoroutine(WaitForCompletePuzzle());
//    }

//    private IEnumerator WaitForCompletePuzzle()
//    {
//        yield return new WaitForSeconds(1.5f);

//        isPuzzleComplete = true;
//        PendantEvent.OnPuzzleSolved?.Invoke();
//    }
//}


using System.Collections;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private int maxPieceCount = 12;
    private int currentCount = 0;
    public bool isPuzzleComplete = false;
    public void NotifyPieceSnapped()
    {
        if (isPuzzleComplete) return;

        currentCount++;
        if (currentCount >= maxPieceCount)
        {
            CompletePuzzle();
        }
    }

    public void CompletePuzzle()
    {
        if (isPuzzleComplete) return;

        StartCoroutine(WaitForCompletePuzzle());
    }
    private IEnumerator WaitForCompletePuzzle()
    {
        yield return new WaitForSecondsRealtime(1f);

        isPuzzleComplete = true;

        PendantEvent.OnPuzzleSolved?.Invoke();
    }
}
