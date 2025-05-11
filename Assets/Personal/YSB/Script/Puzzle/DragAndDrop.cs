using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private GameObject selectPiece;
    [SerializeField] private LayerMask puzzleLayer;
    [SerializeField] private PuzzleController controller;
    void Update()
    {
        HandleMouseDown();
        HandleDragging();
        HandleMouseUp();
    }
    private void HandleMouseDown()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        RaycastHit2D hit = Physics2D.Raycast(
        Camera.main.ScreenToWorldPoint(Input.mousePosition),
        Vector2.zero,
        Mathf.Infinity,
        puzzleLayer);

        if (hit.collider != null && hit.transform.CompareTag("Puzzle"))
        {
            PuzzlePiece pieceScript = hit.transform.GetComponent<PuzzlePiece>();
            if (pieceScript != null && !pieceScript.isLocked)
            {
                selectPiece = hit.transform.gameObject;
            }
        }
    }
    private void HandleDragging()
    {
        if (selectPiece == null || !Input.GetMouseButton(0)) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = selectPiece.transform.position.z;
        selectPiece.transform.position = mousePos;
    }

    private void HandleMouseUp()
    {
        if (!Input.GetMouseButtonUp(0) || selectPiece == null) return;

        Physics2D.SyncTransforms();

        PuzzlePiece pieceScript = selectPiece.GetComponent<PuzzlePiece>();
        if (pieceScript != null)
        {
            bool snapped = pieceScript.TrySnap();
            if (snapped)
            {
                controller.NotifyPieceSnapped();
            }
        }

        selectPiece = null;
    }

}
