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
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
            if (hit.transform.CompareTag("Puzzle"))
            {
                selectPiece = hit.transform.gameObject;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            selectPiece = null;
        }

        if(selectPiece != null)
        {
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectPiece.transform.position  = new Vector3(mousePoint.x, mousePoint.y, 0);
        }
    }
}
