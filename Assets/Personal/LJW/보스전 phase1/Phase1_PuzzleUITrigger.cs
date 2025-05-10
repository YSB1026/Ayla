using UnityEngine;

public class Phase1_PuzzleUITrigger : MonoBehaviour
{
    [SerializeField] private GameObject puzzleUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ayla"))
        {
            Debug.Log("ÆÛÁñ À§Ä¡ µµÂø! UI Ç¥½ÃµÊ");
            if (puzzleUI != null)
                puzzleUI.SetActive(true);
        }
    }
}
