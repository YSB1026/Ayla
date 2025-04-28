using UnityEngine;

public class LightScript : MonoBehaviour
{
    private LightColor lightColor;

    private Light lightCompo;
<<<<<<< HEAD
=======
    [SerializeField]private GameObject go = null;
>>>>>>> parent of f55d700 (LightScript)

    private void Start()
    {
        lightCompo = GetComponent<Light>();
    }

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Object"))
		{
		    LightAbility(collision);		
		}
	}

    public void ChangeLightColer(LightColor _lightColor)//Light의 enum과 color를 바뀐다
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

    private void LightAbility(Collider2D collision)//스페이스바를 누르면 LightColor에 따른 능력이 발휘된다
    {
        if(Input.GetKey(KeyCode.Space))
        {
            switch (lightColor)
            {
                case LightColor.WHITE:
                    break;
                case LightColor.BLACK:
                    break;
                case LightColor.RED:
                    break;
                case LightColor.GREEN:
                    break;
                case LightColor.BLUE:
                    break;
                default:
                    break;
            }
        }
    }
}
