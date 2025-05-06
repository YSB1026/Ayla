using UnityEngine;

public class LightAbillity : MonoBehaviour
{
	[SerializeField] private LightColor lightColor;

	private Light lightCompo;
	[SerializeField] private GameObject go = null;

	private void Start()
	{
		lightCompo = GetComponent<Light>();
	}

	private void Update()
	{
		if (go != null)
		{
			LightAbility();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("FireTrigger") && (lightColor == LightColor.RED || lightColor == LightColor.BLACK))
		{
			go = collision.gameObject;
		}

		if (collision.CompareTag("Enemy") && lightColor == LightColor.BLUE)
		{
			go = collision.gameObject;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("FireTrigger") && (lightColor == LightColor.RED || lightColor == LightColor.BLACK))
		{
			go = null;
		}
	}

	public void ChangeLightColer(LightColor _lightColor)
	{
		lightColor = _lightColor;

		switch (lightColor)
		{
			case LightColor.WHITE:
				lightCompo.color = Color.white;
				break;
			case LightColor.BLACK:
				lightCompo.color = Color.black;
				break;
			case LightColor.RED:
				lightCompo.color = Color.red;
				break;
			case LightColor.GREEN:
				lightCompo.color = Color.green;
				break;
			case LightColor.BLUE:
				lightCompo.color = Color.blue;
				break;
			default:
				break;
		}
	}

	private void LightAbility()
	{
		if (Input.GetKey(KeyCode.F))
		{
			switch (lightColor)
			{
				case LightColor.WHITE:
					break;
				case LightColor.BLACK:
					go?.GetComponent<FirebleObject>().FireOff();
					break;
				case LightColor.RED:
					go.GetComponent<FirebleObject>().FireOn();
					break;
				case LightColor.GREEN:
					break;
				case LightColor.BLUE:
					go?.GetComponent<TestEnemy>().Slow();
					break;
				default:
					break;
			}
		}
	}
}
