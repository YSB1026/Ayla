using System.Collections;
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

    private IEnumerator WindRoutine()
    {
        yield return new WaitForSeconds(windInterval);  // 시작하자마자 바람 불기 금지

        while (phaseActive)
        {
            // 1. 바람 애니메이션 켜기
            if (windEffect != null)
                windEffect.SetActive(true);

            // 2. 왼쪽부터 촛불 꺼지기
            List<CandleTrigger> litCandles = candles
                .Where(c => c.isLit)
                .OrderBy(c => c.transform.position.x)
                .ToList();

            foreach (CandleTrigger candle in litCandles)
            {
                candle.TurnOff();
                yield return new WaitForSeconds(1f); // 1초 간격으로 소등
            }

            // 3. 10초 후 바람 효과 끄기
            yield return new WaitForSeconds(10f); // wind 연출 지속
            if (windEffect != null)
                windEffect.SetActive(false);

            // 4. 다음 바람까지 간격 줄이기
            windInterval *= 0.85f;
            windInterval = Mathf.Max(3f, windInterval);
            yield return new WaitForSeconds(windInterval);
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
