using System;
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
            ActivePuzzle(true);
        }
    }

    private void ActivePuzzle(bool isActive)
    {
        if (puzzleObject != null)
        {
            puzzleObject.SetActive(isActive);
        }
        if (isActive)
        {
            GameManager.Instance.SetPlayerControlEnabled(false);
        }
    }

    private void LoadNextScene()//���� ��Ʈ�ѷ����� action�� ���� ȣ���ϴ� �Լ�����.
    {
        ActivePuzzle(false);
        Destroy(gameObject);

        GameManager.Instance.OnPendantCollected(pendantColor, sceneName);
        GameManager.Instance.SetPlayerControlEnabled(true);

        if (eventAfterRecallScene != null)
        {
            eventAfterRecallScene.SetActive(true);
        }
    }
}
