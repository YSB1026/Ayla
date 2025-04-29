using UnityEngine;

public class FireTrigger : MonoBehaviour
{
    private Animator anim;
    private  Light lightCompo;

	[SerializeField] private GameObject FireObject;

	[SerializeField] private bool defaultFire;
	[SerializeField] private float luminosity;

	private bool isFire;

	private void Start()
	{
		InitComponent();
		InitFire();
	}

	private void Update()
	{
		FideInLight();
	}

	private void InitComponent()
	{
		anim = GetComponentInChildren<Animator>();
		lightCompo = GetComponentInChildren<Light>();
	}

	private void InitFire()
	{
		isFire = defaultFire;
		lightCompo.intensity = 0;
		FireObject.SetActive(defaultFire);
	}
	private void FideInLight()
	{
		if(isFire)
		{
			lightCompo.intensity = Mathf.Lerp(lightCompo.intensity, luminosity, Time.deltaTime);
		}
	}

	public void FireOn()
    {
		isFire = true;
		FireObject.SetActive(isFire);
		//anim.SetBool("Fire", isFire);
	}

	public void FireOff()
	{
		isFire = false;
		//anim.SetBool("Fire", isFire);
		lightCompo.intensity = 0f;
		FireObject.SetActive(isFire);
	}
}
