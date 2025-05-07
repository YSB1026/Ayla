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

	[Header("Object")]
	[SerializeField] private GameObject movedObject;
	[SerializeField] private Transform destination;

	[Header("Info")]
	[SerializeField] private float moveSpeed;
	[SerializeField] private TrickMoveType moveType;
	[SerializeField] private bool isDestroy;

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
				movedObject.transform.position = Vector3.MoveTowards(movedObject.transform.position, destination.position, moveSpeed * Time.deltaTime);
				break;
			case TrickMoveType.LERP:
				movedObject.transform.position = Vector3.Lerp(movedObject.transform.position, destination.position, moveSpeed * Time.deltaTime);
				break;
			case TrickMoveType.TELEPORT:
				movedObject.transform.position = destination.position;
				break;
		}

	}

	private void DestroySelf()
	{
		if (Vector3.Distance(movedObject.transform.position, destination.position) < DESTROY_DISTANCE && isDestroy)
			Destroy(gameObject);
	}
}
