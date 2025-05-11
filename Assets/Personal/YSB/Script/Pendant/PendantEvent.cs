using System;
using System.Collections;
using UnityEngine;
using static LightColorController;

public class PendantEvent : MonoBehaviour
{
    //퍼즐
    [Header("획득할 빛")]
    public ColorOption pendantColor;

    [Header("퍼즐")]
    [SerializeField] private GameObject puzzleObject;

    [Header("회상 씬 트리거")]
    [SerializeField] private GameObject recallSceneTrigger;

    private void OnValidate()
    {
        if(puzzleObject == null)
        {
            Debug.LogError($"{gameObject.name} 퍼즐 넣어주세요!!");
        }

        //if (recallSceneTrigger == null || recallSceneTrigger.activeSelf)
        //{
        //    Debug.LogWarning($"{gameObject.name} 회상씬 넣어주세요!!, 비활성화도 해주세요");
        //}
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

    private void OnPendantEvent()//퍼즐 컨트롤러에서 action을 통해 호출하는 함수에요.
    {
        StartCoroutine(ActivePuzzle(false));

        GameManager.Instance.OnPendantCollected(pendantColor);
        GameManager.Instance.SetPlayerControlEnabled(true);

        recallSceneTrigger.SetActive(true);
        Destroy(gameObject);
    }
}
