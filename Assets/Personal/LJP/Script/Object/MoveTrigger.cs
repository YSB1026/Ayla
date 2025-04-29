using UnityEngine;

public enum TrickMoveType
{
	MOVETOWARDS,
	LERP,
	TELEPORT
}

public class MoveTrigger : MonoBehaviour
{
	private const float DESTROY_DISTANCE = 0.01f;

	[SerializeField] private GameObject go;
	[SerializeField] private Transform target;
	[SerializeField] private float moveSpeed;
	[SerializeField] private TrickMoveType moveType;

	private bool isTrigger = false;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			isTrigger = true;
		}
	}

	private void Update()
	{
		if (isTrigger)
		{
			MoveTrick();
		}

		DestroySelf();
	}

	private void MoveTrick()
	{
		switch (moveType)
		{
			case TrickMoveType.MOVETOWARDS:
				go.transform.position = Vector3.MoveTowards(go.transform.position, target.position, moveSpeed * Time.deltaTime);
				break;
			case TrickMoveType.LERP:
				go.transform.position = Vector3.Lerp(go.transform.position, target.position, moveSpeed * Time.deltaTime);
				break;
			case TrickMoveType.TELEPORT:
				go.transform.position = target.position;
				break;
		}

	}

	private void DestroySelf()
	{
		if (Vector3.Distance(go.transform.position, target.position) < DESTROY_DISTANCE)
			Destroy(gameObject);
	}
}
