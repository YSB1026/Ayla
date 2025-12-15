using UnityEngine;

public class Door : InteractableObject
{
	[SerializeField] private Door Exit;

	public void SetExit(Door door)
	{
		Exit = door;
	}

	public void EnterRoom()
	{
		player.transform.position = Exit.transform.position;
	}

	protected override void Interact()
	{
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, layerMask);

		if (hit.collider != null && hit.collider.gameObject == this.gameObject)
		{
			EnterRoom();
		}

	}
}
