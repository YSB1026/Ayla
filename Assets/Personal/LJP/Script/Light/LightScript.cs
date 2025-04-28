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

    public void ChangeLightColer(LightColor _lightColor)//Light�� enum�� color�� �ٲ��
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

    private void LightAbility(Collider2D collision)//�����̽��ٸ� ������ LightColor�� ���� �ɷ��� ���ֵȴ�
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
