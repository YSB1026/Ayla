using UnityEngine;

public class Light : MonoBehaviour
{
    private LightColor lightColor;

    private void Start()
    {
        
    }

    private void Update()
    {
        LightAbility();
    }

    public void ChangeLightColer(LightColor _lightColor) => lightColor = _lightColor;

    private void LightAbility()//스페이스바를 누르면 LightColor에 따른 능력이 발휘된다
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
