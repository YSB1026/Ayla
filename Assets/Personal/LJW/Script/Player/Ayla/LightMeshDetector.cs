using UnityEngine;
using System.Collections.Generic;

namespace YSB
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LightMeshDetector : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boxCollider;

        // 성능 최적화를 위해 캐싱
        private List<ILightReactive> cachedReactives = new List<ILightReactive>();

        private void Start()
        {
            // 게임 시작 시 씬에 있는 ILightReactive들 미리 찾기
            var monos = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var mb in monos)
            {
                if (mb is ILightReactive reactive)
                {
                    cachedReactives.Add(reactive);
                }
            }
        }

        public List<ILightReactive> Detect()
        {
            if (boxCollider == null) return null;

            List<ILightReactive> inLightReactives = new List<ILightReactive>();
            // var reactives = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

            /*foreach (var reactive in cachedReactives)
            {
                MonoBehaviour mb = reactive as MonoBehaviour;
                if (mb == null) continue;

                // 1. 내 범위(Collider) 안에 적의 중심점(Position)이 들어왔는지 확인
                bool inside = boxCollider.OverlapPoint(mb.transform.position);

                // 2. 상태값 갱신 (명령 실행 X)
                reactive.IsInLight = inside;

                if (inside)
                    inLightReactives.Add(reactive);
            }*/
            foreach (var reactive in cachedReactives)
            {
                MonoBehaviour mb = reactive as MonoBehaviour;
                if (mb == null) continue;

                // 1. 내 범위(Collider) 안에 적의 중심점(Position)이 들어왔는지 확인
                bool inside = boxCollider.OverlapPoint(mb.transform.position);

                // 2. 상태값 갱신 
                reactive.IsInLight = inside;

                if (inside)
                    inLightReactives.Add(reactive);
            }

            //Debug.Log($"Detected {inLightReactives.Count} light reactives.");
            return inLightReactives;
        }

        private void OnDrawGizmos()
        {
            if (boxCollider == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);

            // 감지 대상들의 '중심점'을 빨간 점
            if (cachedReactives != null)
            {
                Gizmos.color = Color.red;
                foreach (var r in cachedReactives)
                {
                    if (r is MonoBehaviour mb)
                    {
                        Gizmos.DrawSphere(mb.transform.position, 0.2f);
                    }
                }
            }
        }
    }
}
