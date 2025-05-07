using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private GameObject selectPiece;
    [SerializeField] private LayerMask puzzleLayer;
    [SerializeField] private PuzzleController controller;

    [SerializeField] private int pieceCount = 0;
    [SerializeField] private int maxPieceCount = 12;

    void Update()
    {
        // 퍼즐 클릭
        if (Input.GetMouseButtonDown(0))
        {
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

        // 드래그 중
        if (selectPiece != null && Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = selectPiece.transform.position.z;
            selectPiece.transform.position = mousePos;
        }

        // 드래그 끝났을 때
        if (Input.GetMouseButtonUp(0) && selectPiece != null)
        {
            // 위치 이동 후 물리 업데이트 동기화
            Physics2D.SyncTransforms();

            PuzzlePiece pieceScript = selectPiece.GetComponent<PuzzlePiece>();
            if (pieceScript != null)
            {
                bool snapped = pieceScript.TrySnap();
                if (snapped)
                {
                    pieceCount++;
                    if (pieceCount == maxPieceCount)
                    {
                        controller.CompletePuzzle();
                    }
                }
            }

            selectPiece = null;
        }
    }
}
