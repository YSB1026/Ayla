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

    [Header("���� �н�")]
    public GameObject bossClonePrefab; // �ν����Ϳ��� �Ҵ�

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

    public void SpawnClone(Vector3 spawnPosition)
    {
        GameObject clone = Instantiate(bossClonePrefab, spawnPosition, Quaternion.identity);
    }

    private IEnumerator WindRoutine()
    {
        yield return new WaitForSeconds(windInterval);  // �������ڸ��� �ٶ� �ұ� ����

        while (phaseActive)
        {
            // 1. wind �ִϸ��̼�
            if (windEffect != null)
                windEffect.SetActive(true);

            // 2. ��ƾ : ��� ������ ������� ����. �ٶ� �� �� �ѹ��� ������ �к�
            List<int[]> windOffPatterns = new List<int[]>
            {
                new int[] { 0, 4 },
                new int[] { 2 },
                new int[] { 1, 3 }
            };


            foreach (int[] pattern in windOffPatterns)
            {
                // �� ���Ͽ� �ش��ϴ� �кҵ� �߿���, ��ġ x���� �������� ����
                List<CandleTrigger> candlesToTurnOff = pattern
                    .Where(idx => idx >= 0 && idx < candles.Count && candles[idx].isLit)
                    .Select(idx => candles[idx])
                    .OrderBy(c => c.transform.position.x)
                    .ToList();

                foreach (var candle in candlesToTurnOff)
                {
                    candle.TurnOff();
                    yield return new WaitForSeconds(1f); // �� �� ���� ������ 1�� ���
                }

                // ���� �ϳ� ���� ������ 3�� ���
                yield return new WaitForSeconds(3f);
            }

            // ��ƾ �ϳ� ���� �� wind ���� ���� + windInterval ���̱�
            if (windEffect != null)
                windEffect.SetActive(false);

            yield return new WaitForSeconds(10f); // wind ȿ�� ���� �� ���
            windInterval *= 0.85f;
            windInterval = Mathf.Max(5f, windInterval);

            yield return new WaitForSeconds(windInterval); // ���� ��ƾ ������ ���
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
