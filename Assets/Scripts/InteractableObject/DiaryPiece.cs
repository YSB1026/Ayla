using UnityEngine;

public class DiaryPiece : InteractableObject
{
	[SerializeField] private int PieceIndex;

	protected override void Interact()
	{
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, layerMask);

		if (hit.collider != null && hit.collider.gameObject == this.gameObject)
		{
			UIManager.Instance.ShowViewer(ViewerUIType.Diary);
			UIManager.Instance.GetDiaryUI().UnlockDiaryPiece(PieceIndex);
			Destroy(this.gameObject);
		}
	}
	
}
