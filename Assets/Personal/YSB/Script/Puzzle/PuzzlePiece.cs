using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private GameObject targetPiece;
    [SerializeField] private float snapThreshold = 3f;
    [SerializeField] public bool isLocked = false;

    public bool TrySnap()
    {
        if (targetPiece == null) return false;

        float distance = Vector3.Distance(transform.position, targetPiece.transform.position);
        if (distance <= snapThreshold)
        {
            transform.position = targetPiece.transform.position;
            isLocked = true;
            return true;
        }
        return false;
    }
}
