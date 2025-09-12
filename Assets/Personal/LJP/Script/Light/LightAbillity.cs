using UnityEngine;

public enum LightColor
{
	WHITE,
	BLACK,
	RED,
	GREEN,
	BLUE
}

public class LightAbillity : MonoBehaviour
{
	[Header("스크립트 및 컴포넌트")]
	[SerializeField] private Ayla ayla;
	[SerializeField] private LightColor lightColor;
	[SerializeField] private Collider2D lightCollier;
	private HardLight2D lightRenderer;

	[Header("능력 밸류")]
	[SerializeField] private float stunTime = 1.0f;
	[SerializeField] private float slowRate = 0.3f;

	[SerializeField] private GameObject go = null;

	private void Start()
	{
		lightRenderer = GetComponentInChildren<HardLight2D>();
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
				lightRenderer.Color = Color.white;
				break;
			case LightColor.BLACK:
				lightRenderer.Color = Color.black;
				break;
			case LightColor.RED:
				lightRenderer.Color = Color.red;
				break;
			case LightColor.GREEN:
				lightRenderer.Color = Color.green;
				break;
			case LightColor.BLUE:
				lightRenderer.Color = Color.blue;
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
					go.GetComponent<Enemy>().ApplyStun(stunTime);
					break;
				case LightColor.RED:
					go.GetComponent<FirebleObject>().FireOn();
					break;
				case LightColor.GREEN:
					break;
				case LightColor.BLUE:
					go?.GetComponent<Enemy>().ApplySlow(slowRate);
					break;
				default:
					break;
			}
		}
	}
}
