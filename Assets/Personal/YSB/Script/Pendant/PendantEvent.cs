using System;
using System.Collections;
using UnityEngine;
using static LightColorController;

public class PendantEvent : MonoBehaviour
{
    [Header("펜던트 색상")]
    public ColorOption pendantColor;

    [Header("퍼즐 오브젝트")]
    [SerializeField] private GameObject puzzleObject;

    [Header("회상 장면 트리거")]
    [SerializeField] private GameObject recallSceneTrigger;

    private void OnValidate()
    {
        if(puzzleObject == null)
        {
            Debug.LogError($"{gameObject.name} 퍼즐 오브젝트 할당 안됨");
        }
    }
    private void OnEnable()
    {
        if (puzzleObject.activeSelf) puzzleObject.SetActive(false);
        if(recallSceneTrigger.activeSelf) recallSceneTrigger.SetActive(false);
    }
    private void Start()
    {
        var controller = puzzleObject.GetComponent<PuzzleController>();
        if (controller != null)
        {
            controller.Init(OnPendantEvent);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.SetPlayerControlEnabled(false);
            StartCoroutine(ActivePuzzle(true));
        }
    }

    private IEnumerator ActivePuzzle(bool isActive)
    {
        yield return new WaitForSeconds(2f);
        if (puzzleObject != null)
        {
            puzzleObject.SetActive(isActive);
        }
    }

    private void OnPendantEvent()//펜던트 수집 시 호출되는 이벤트
    {
        StartCoroutine(ActivePuzzle(false));

        GameManager.Instance.OnPendantCollected(pendantColor);
        GameManager.Instance.SetPlayerControlEnabled(true);

        recallSceneTrigger.SetActive(true);
        Destroy(gameObject);
    }
}
