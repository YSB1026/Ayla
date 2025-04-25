using UnityEngine;

public enum MoveType
{
	MOVETOWARDS,
	LERP,
	TELEPORT
}

public class TriggerTrick : MonoBehaviour
{
	[SerializeField] private GameObject go;
	[SerializeField] private Transform target;
	[SerializeField] private float moveSpeed;
	[SerializeField] private MoveType moveType;

	private bool isTrigger = false;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
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

	private void MoveTrick()//�̵�Ÿ�Կ� ���� ������Ʈ �̵�
	{
		switch(moveType)
		{
			case MoveType.MOVETOWARDS:
				go.transform.position = Vector3.MoveTowards(go.transform.position, target.position, moveSpeed * Time.deltaTime);
				break;
			case MoveType.LERP:
				go.transform.position = Vector3.Lerp(go.transform.position, target.position, moveSpeed * Time.deltaTime);
				break;
			case MoveType.TELEPORT:
				go.transform.position = target.position;
				break;
		}

	}

	private void DestroySelf()//Ÿ�ٿ� ��������� �����θ� �ı�
	{
		if (Vector3.Distance(go.transform.position, target.position) < 0.01)
			Destroy(gameObject);
	}
}
