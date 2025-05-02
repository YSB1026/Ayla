using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
	private Rigidbody2D rb;
	private Collider2D col;

	[SerializeField] private float moveSpeed = 3f;
	private bool defualtTrigger;

	private void Awake()
	{
		InitComponent();		
	}
	private void Start()
	{
		defualtTrigger = col.isTrigger;
		FreezeObject(true);
	}

	private void InitComponent()
	{
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
	}

	public void FreezeObject(bool isFreezed)//True�̸� Rigidbody�� �󸮰� false�̸� Rigidbody�� Ǭ��
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

	public void SetTrigger(bool _isTrigger)
	{
		if(_isTrigger)
			col.isTrigger = defualtTrigger;
		else 
			col.isTrigger = _isTrigger;
	}

	public void MoveObject(float moveDir)//moveDir�������� ������Ʈ�� �����δ�
	{
		transform.position += new Vector3(moveSpeed * moveDir * Time.deltaTime, 0, 0);
	}
}
