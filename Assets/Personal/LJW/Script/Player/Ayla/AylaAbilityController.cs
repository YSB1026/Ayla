using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YSB
{
    public class AylaAbilityController : MonoBehaviour
    {
        [SerializeField] private LightColorController lightController;
        [SerializeField] private LightMeshDetector lightMeshDetector;
        [SerializeField] private List<ILightReactive> inLightReactives;
        private void Update()
        {
            // GameManager 자체가 아직 없으면 그냥 리턴
            if (GameManager.Instance == null)
                return;

            // 플레이어 조작이 막혀 있으면 리턴
            if (!GameManager.Instance.IsPlayerControlEnabled)
                return;

            if (Keyboard.current == null) return;

            // 탭 키로 색(능력) 순환
            if (Keyboard.current.tabKey.wasPressedThisFrame)
            {
                if (lightController != null)
                {
                    lightController.NextAbilityColor();
                    Debug.Log($"Ayla ability color changed to: {lightController.currentColor}");
                }
            }

            // E 클릭 시, 현재 빛 영역 안의 리액티브들을 탐색해서 반응 적용
            if (Input.GetKeyDown(KeyCode.E))
            {
                inLightReactives = lightMeshDetector.Detect();
                lightController.ChangeRangeWithFade();
                Debug.Log($"Detected {inLightReactives.Count} light reactives.");
            }

            // 감지된 대상에게 실제 반응 적용
            if (inLightReactives != null && inLightReactives.Count > 0)
            {
                Debug.Log($"Detected {inLightReactives.Count} light reactives.");
                foreach (var reactive in inLightReactives)
                {
                    reactive.ApplyLightReaction();
                }
                inLightReactives.Clear();
            }
        }
    }
}
