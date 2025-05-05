using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private GameObject selectPiece;
    void Start()
    {
        
    }

    void Update()
    {
        //퍼즐 드래그 로직
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.transform.CompareTag("Puzzle"))
            {
                PuzzlePiece pieceScript = hit.transform.GetComponent<PuzzlePiece>();
                if (pieceScript != null && !pieceScript.isLocked)
                {
                    selectPiece = hit.transform.gameObject;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            selectPiece = null;
        }

        //puzzle move 로직
        if (selectPiece != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = selectPiece.transform.position.z;
            selectPiece.transform.position = mousePos;
        }

        if (Input.GetMouseButtonUp(0) && selectPiece != null)
        {
            PuzzlePiece pieceScript = selectPiece.GetComponent<PuzzlePiece>();
            if (pieceScript != null)
            {
                bool snapped = pieceScript.TrySnap();
                if (snapped)
                {
                    selectPiece = null;
                }
            }
        }
    }
}
