using UnityEngine;

public class ShadowAbility : MonoBehaviour
{
    private void Update()
    {
        // GameManager 체크 (안전장치)
        if (GameManager.Instance == null) return;

        // 플레이어 조작 가능 여부 확인
        if (!GameManager.Instance.IsPlayerControlEnabled) return;

        // Q키 입력 감지 (Input.GetKeyDown 방식)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateShadow();
        }
    }

    // 능력이 발동되는 함수
    private void ActivateShadow()
    {
        Debug.Log("Shadow Ability Activated! (Legacy Input)");

        // TODO: 나중에 여기에 그림자 능력 내용 채워넣기
    }
}