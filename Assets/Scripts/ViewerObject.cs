using UnityEngine;
using UnityEngine.UI;

public enum ViewerUIType
{
    Safe,
    Diary,
	Pendant
}

public class ViewerObject : MonoBehaviour
{
    [SerializeField] private ViewerUIType viewerType;
	[SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject outline;
    public ViewerUIType ViewerType { get { return viewerType; } }

    private Player player;

	private void Start()
	{
		outline.SetActive(false);
	}

	private void Update()
	{
		if(player != null)
        {
			if (Input.GetMouseButtonDown(0))
			{
				Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, layerMask);

				if (hit.collider != null && hit.collider.gameObject == this.gameObject)
				{
					Debug.Log("성공");
					UIManager.Instance.ShowViewer(viewerType);
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.TryGetComponent<Player>(out player))
        {
            ShowOutline();
        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.TryGetComponent<Player>(out player))
        {
            HideOutline();
            player = null;
        }
	}

	private void ShowOutline()
    {
        outline.SetActive(true);
    }

	private void HideOutline()
    {
        outline.SetActive(false);
    }
}
