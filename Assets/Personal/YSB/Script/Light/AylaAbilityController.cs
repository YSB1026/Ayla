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
            if (!GameManager.Instance.IsPlayerControlEnabled) return;

            if (Keyboard.current == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                inLightReactives = lightMeshDetector.Detect();
                lightController.ChangeRangeWithFade();
                //Debug.Log($"Detected {inLightReactives.Count} light reactives.");
            }
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
