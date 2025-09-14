using UnityEngine;
using UnityEngine.InputSystem;

public class AylaAbilityController : MonoBehaviour
{
    [SerializeField] private LightColorController lightColorController;

    [SerializeField] private Keyboard abilityKey;

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (abilityKey.IsPressed())
        {
            lightColorController.ChangeRangeWithFade();
            ActivateLightAbility();
        }
    }

    private void ActivateLightAbility()
    {
        switch (lightColorController.currentColor)
        {
            case ColorOption.Red:
                // 빨간색 능력 활성화 코드
                Debug.Log("Red Ability Activated");
                break;
                break;
            case ColorOption.Blue:
                // 파란색 능력 활성화 코드
                Debug.Log("Blue Ability Activated");
                break;
            case ColorOption.Green:
                // 초록색 능력 활성화 코드
                Debug.Log("Green Ability Activated");
                break;
            default:
                break;
        }
    }
}
