using System;
using System.Collections;
using UnityEngine;
using static LightColorController;

public class PendantEvent : MonoBehaviour
{
    //����
    [Header("ȹ���� ��")]
    public ColorOption pendantColor;

    [Header("ȸ�� ��")]
    [SerializeField] private string sceneName;

    [Header("����")]
    [SerializeField] private GameObject puzzleObject;

    [Header("ȸ��� ���� �̺�Ʈ Ʈ����")]
    [SerializeField] private GameObject eventAfterRecallScene;

    public static Action OnPuzzleSolved;

    private void OnValidate()
    {
        if(puzzleObject == null)
        {
            Debug.LogError($"{gameObject.name} ���� �־��ּ���!!");
        }
    }

    private void Start()
    {
        if (!puzzleObject.activeSelf) puzzleObject.SetActive(false);
    }

    private void OnEnable()
    {
        Debug.Log("PendantEvent enabled, subscribing to OnPuzzleSolved");
        OnPuzzleSolved += LoadNextScene;
    }

    private void OnDisable()
    {
        OnPuzzleSolved -= LoadNextScene;
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

    private void LoadNextScene()//���� ��Ʈ�ѷ����� action�� ���� ȣ���ϴ� �Լ�����.
    {
        StartCoroutine(ActivePuzzle(false));
        Destroy(gameObject);

        GameManager.Instance.OnPendantCollected(pendantColor, sceneName);
        GameManager.Instance.SetPlayerControlEnabled(true);

        if (eventAfterRecallScene != null)
        {
            eventAfterRecallScene.SetActive(true);
        }
    }
}
