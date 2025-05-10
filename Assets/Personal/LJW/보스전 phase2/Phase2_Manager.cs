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

    [Header("Wind ����")]
    public GameObject windEffect;

    private void Awake()
    {
        Instance = this;
    }

    public void StartPhase()
    {
        phaseActive = true;

        Debug.Log($"[Phase2Manager] ���� - ��ϵ� �к� ��: {candles.Count}");

        // �к� �ϳ� Ű�� ����
        if (candles.Count > 0)
        {
            var first = candles[0];
            Debug.Log($"[Phase2Manager] ù ��° �к� �̸�: {first.name}, isLit: {first.isLit}");

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
            Debug.Log("[TryLightCandle] ������ �к� ����");
        }

        candle.TurnOn();
    }

    private IEnumerator WindRoutine()
    {
        yield return new WaitForSeconds(windInterval);  // �������ڸ��� �ٶ� �ұ� ����

        while (phaseActive)
        {
            // 1. �ٶ� �ִϸ��̼� �ѱ�
            if (windEffect != null)
                windEffect.SetActive(true);

            // 2. ���ʺ��� �к� ������
            List<CandleTrigger> litCandles = candles
                .Where(c => c.isLit)
                .OrderBy(c => c.transform.position.x)
                .ToList();

            foreach (CandleTrigger candle in litCandles)
            {
                candle.TurnOff();
                yield return new WaitForSeconds(1f); // 1�� �������� �ҵ�
            }

            // 3. 10�� �� �ٶ� ȿ�� ����
            yield return new WaitForSeconds(10f); // wind ���� ����
            if (windEffect != null)
                windEffect.SetActive(false);

            // 4. ���� �ٶ����� ���� ���̱�
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
        phase2Controller.EndPhase2();   // �߰� �̺�Ʈ ȣ��
        Debug.Log("Phase2 ����!");
    }

    private void Update()
    {
        if (!phaseActive) return;

        // üũ: ��� �к� ������ ���
        if (candles.All(c => !c.isLit))
        {
            Debug.Log("��� �к� ���� �� �÷��̾� ���");
            player.GetComponent<Player>().Die();

            phaseActive = false;    // �ߺ� ȣ�� ����
        }
    }
}
