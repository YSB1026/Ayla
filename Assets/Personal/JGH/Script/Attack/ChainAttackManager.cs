using UnityEngine;
using System.Collections;

public class ChainAttackManager : MonoBehaviour
{
    public GameObject warningLinePrefab;
    public GameObject chainPrefab;

    public float warningDuration = 1f;
    public float attackInterval = 2f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            Vector2 targetPos = player.position;

            // 1. 경고선 표시
            GameObject warning = Instantiate(warningLinePrefab, targetPos, Quaternion.identity);
            yield return new WaitForSeconds(warningDuration);

            Destroy(warning); // 경고선 사라짐

            // 2. 경고선 타이밍 끝난 뒤 사슬 공격 시작
            Vector2 spawnPos = targetPos + Vector2.up * 10f;
            GameObject chain = Instantiate(chainPrefab, spawnPos, Quaternion.identity);

            // ChainAttack 스크립트에서 내려오게 처리
            chain.GetComponent<ChainAttack>().StartAttack(spawnPos, targetPos);

            // 3. 공격 간격 대기
            yield return new WaitForSeconds(attackInterval);
        }
    }
}
