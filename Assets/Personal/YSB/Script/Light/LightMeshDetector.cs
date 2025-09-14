using UnityEngine;
using System.Collections.Generic;

namespace YSB
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LightMeshDetector : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boxCollider;

        public List<ILightReactive> Detect()
        {
            if (boxCollider == null) return null;

            List<ILightReactive> inLightReactives = new List<ILightReactive>();
            var reactives = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

            foreach (var mb in reactives)
            {
                if (mb is ILightReactive reactive)
                {
                    bool inside = boxCollider.OverlapPoint(mb.transform.position);
                    reactive.IsInLight = inside;

                    if (inside)
                        inLightReactives.Add(reactive);
                }
            }

            //Debug.Log($"Detected {inLightReactives.Count} light reactives.");
            return inLightReactives;
        }
    }
}
