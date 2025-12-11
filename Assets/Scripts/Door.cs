using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
	[SerializeField] private Door Exit;
	private Player player;

	private Outline outline;

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

	public void SetExit(Door door)
	{
		Exit = door;
	}

	public void EnterRoom()
	{
		player.transform.position = Exit.transform.position;
	}

	private void ShowOutline()
	{
		outline.enabled = true;
	}

	private void HideOutline()
	{
		outline.enabled = false;
	}
}
