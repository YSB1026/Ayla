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

        void OnDrawGizmos()
        {
            var bc = GetComponent<BoxCollider2D>();
            if (bc == null) return;

            Gizmos.color = Color.blue;

            // BoxCollider2D의 네 구석 월드 좌표 구하기
            Vector3 center = bc.transform.TransformPoint(bc.offset);
            Vector3 size = Vector3.Scale(bc.size, bc.transform.lossyScale);

            Vector3 topLeft = center + new Vector3(-size.x, size.y) * 0.5f;
            Vector3 topRight = center + new Vector3(size.x, size.y) * 0.5f;
            Vector3 bottomRight = center + new Vector3(size.x, -size.y) * 0.5f;
            Vector3 bottomLeft = center + new Vector3(-size.x, -size.y) * 0.5f;

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}
