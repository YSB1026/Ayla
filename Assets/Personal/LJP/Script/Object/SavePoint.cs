using UnityEngine;

public class SavePoint : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			SetSavePoint();
		}
	}

	private void SetSavePoint()
	{
		GameManager.Instance.savePoint = transform.position;
		Destroy(gameObject);
	}
}
