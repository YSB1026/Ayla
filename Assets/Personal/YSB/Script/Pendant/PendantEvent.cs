using System;
using System.Collections;
using UnityEngine;
using static LightColorController;

public class PendantEvent : MonoBehaviour
{
    //����
    [Header("ȹ���� ��")]
    public ColorOption pendantColor;

    [Header("����")]
    [SerializeField] private GameObject puzzleObject;

    [Header("ȸ�� �� Ʈ����")]
    [SerializeField] private GameObject recallSceneTrigger;

    private void OnValidate()
    {
        if(puzzleObject == null)
        {
            Debug.LogError($"{gameObject.name} ���� �־��ּ���!!");
        }

        //if (recallSceneTrigger == null || recallSceneTrigger.activeSelf)
        //{
        //    Debug.LogWarning($"{gameObject.name} ȸ��� �־��ּ���!!, ��Ȱ��ȭ�� ���ּ���");
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

    private void OnPendantEvent()//���� ��Ʈ�ѷ����� action�� ���� ȣ���ϴ� �Լ�����.
    {
        StartCoroutine(ActivePuzzle(false));

        GameManager.Instance.OnPendantCollected(pendantColor);
        GameManager.Instance.SetPlayerControlEnabled(true);

        recallSceneTrigger.SetActive(true);
        Destroy(gameObject);
    }
}
