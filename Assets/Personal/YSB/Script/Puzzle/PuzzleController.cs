using System;
using System.Collections;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private int maxPieceCount = 12;
    private int currentCount = 0;
    public bool isPuzzleComplete = false;

    private Action onPuzzleComplete;

    public void Init(Action onCompleteCallback)
    {
        onPuzzleComplete = onCompleteCallback;
    }
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
        yield return new WaitForSeconds(1f);

        isPuzzleComplete = true;
        onPuzzleComplete?.Invoke();
    }
}
