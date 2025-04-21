using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
	private Rigidbody2D rb;

	[SerializeField] private float moveSpeed;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		FreezeObject(true);
	}	

	public void FreezeObject(bool isFreezed)//True�̸� Rigidbody�� �󸮰� false�̸� Rigidbody�� Ǭ��
	{
		if (isFreezed)
		{
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
		else
		{
			rb.constraints = RigidbodyConstraints2D.None;
		}
	}

	public void MoveObject(float moveDir)//moveDir�������� ������Ʈ�� �����δ�
	{
		rb.linearVelocity = new Vector2(moveSpeed * moveDir, rb.linearVelocityY);
	}
}
