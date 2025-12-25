using UnityEngine;

public class DiaryPiece : InteractableObject
{
	[SerializeField] private int PieceIndex;
	[SerializeField] private bool isHiden = false;

	public void SetIsHiden(bool hiden) { isHiden = hiden; }

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		base.OnTriggerEnter2D(collision);

		if(collision.TryGetComponent<MovebleObject>(out var movebleObject))
		{
			isHiden = true;
		}
	}

	protected override void OnTriggerExit2D(Collider2D collision)
	{
		base.OnTriggerExit2D(collision);

		if (collision.TryGetComponent<MovebleObject>(out var movebleObject))
		{
			isHiden = false;
		}
	}

	protected override void Interact()
	{
		if (isHiden == true) return;

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
