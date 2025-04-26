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

	public void FreezeObject(bool isFreezed)//True이면 Rigidbody를 얼리고 false이면 Rigidbody를 푼다
	{
		if (isFreezed)
		{
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
		else
		{
			rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	public void MoveObject(float moveDir)//moveDir방향으로 오브젝트를 움직인다
	{
		transform.position += new Vector3(moveSpeed * moveDir * Time.deltaTime, 0);
	}
}
