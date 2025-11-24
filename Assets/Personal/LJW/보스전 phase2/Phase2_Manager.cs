/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Phase2_Manager : MonoBehaviour
{
    public static Phase2_Manager Instance;

    public List<CandleTrigger> candles = new List<CandleTrigger>();
    public int maxLitCount = 4;

    private float windInterval = 10f;
    private float elapsedTime = 0f;
    private bool phaseActive = false;

    public GameObject player;
    public GameObject teleportScriptObj;
    public Phase2Controller phase2Controller;

    [Header("보스 분신")]
    public GameObject bossClonePrefab; // 인스펙터에서 할당

    [Header("Wind 연출")]
    public GameObject windEffect;

    private void Awake()
    {
        Instance = this;
    }

    public void StartPhase()
    {
        phaseActive = true;

        Debug.Log($"[Phase2Manager] 시작 - 등록된 촛불 수: {candles.Count}");

        // 촛불 하나 키고 시작
        if (candles.Count > 0)
        {
            var first = candles[0];
            Debug.Log($"[Phase2Manager] 첫 번째 촛불 이름: {first.name}, isLit: {first.isLit}");

            first.TurnOn();
        }

        teleportScriptObj.SetActive(true);
        StartCoroutine(WindRoutine());
        StartCoroutine(PhaseTimer());
    }

    public void TryLightCandle(CandleTrigger candle)
    {
        int litCount = candles.Count(c => c.isLit);
        if (litCount >= maxLitCount)
        {
            CandleTrigger oldest = candles.Where(c => c.isLit)
                                          .OrderBy(c => c.litTime)
                                          .First();
            oldest.TurnOff();
            Debug.Log("[TryLightCandle] 오래된 촛불 꺼짐");
        }

        candle.TurnOn();
    }

    public void SpawnClone(Vector3 spawnPosition)
    {
        GameObject clone = Instantiate(bossClonePrefab, spawnPosition, Quaternion.identity);
    }

    private IEnumerator WindRoutine()
    {
        yield return new WaitForSeconds(windInterval);  // 시작하자마자 바람 불기 금지

        while (phaseActive)
        {
            // 1. wind 애니메이션
            if (windEffect != null)
                windEffect.SetActive(true);

            // 2. 루틴 : 모든 패턴을 순서대로 돌기. 바람 불 때 한번에 꺼지는 촛불
            List<int[]> windOffPatterns = new List<int[]>
            {
                new int[] { 0, 4 },
                new int[] { 2 },
                new int[] { 1, 3 }
            };


            foreach (int[] pattern in windOffPatterns)
            {
                // 이 패턴에 해당하는 촛불들 중에서, 위치 x기준 오름차순 정렬
                List<CandleTrigger> candlesToTurnOff = pattern
                    .Where(idx => idx >= 0 && idx < candles.Count && candles[idx].isLit)
                    .Select(idx => candles[idx])
                    .OrderBy(c => c.transform.position.x)
                    .ToList();

                foreach (var candle in candlesToTurnOff)
                {
                    candle.TurnOff();
                    yield return new WaitForSeconds(1f); // 한 개 꺼질 때마다 1초 대기
                }

                // 패턴 하나 끝날 때마다 3초 대기
                yield return new WaitForSeconds(3f);
            }

            // 루틴 하나 종료 → wind 연출 종료 + windInterval 줄이기
            if (windEffect != null)
                windEffect.SetActive(false);

            yield return new WaitForSeconds(10f); // wind 효과 종료 후 대기
            windInterval *= 0.85f;
            windInterval = Mathf.Max(5f, windInterval);

            yield return new WaitForSeconds(windInterval); // 다음 루틴 전까지 대기
        }
    }

    private IEnumerator PhaseTimer()
    {
        while (elapsedTime < 90f)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        EndPhase();
    }

    private void EndPhase()
    {
        phaseActive = false;
        teleportScriptObj.SetActive(false);
        phase2Controller.EndPhase2();   // 추가 이벤트 호출
        Debug.Log("Phase2 종료!");
    }

    private void Update()
    {
        if (!phaseActive) return;

        // 체크: 모든 촛불 꺼지면 사망
        if (candles.All(c => !c.isLit))
        {
            Debug.Log("모든 촛불 꺼짐 → 플레이어 사망");
            player.GetComponent<Player>().Die();

            phaseActive = false;    // 중복 호출 방지
        }
    }
}
*/