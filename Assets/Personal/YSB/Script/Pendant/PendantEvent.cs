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

    public static Action OnPuzzleSolved;

    private void Start()
    {
        if (!puzzleObject.activeSelf) puzzleObject.SetActive(false);
    }

    private void OnEnable()
    {
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
            GameManager.Instance.ChangeState(GameState.Cutscene);
        }
    }

    private void ActivePuzzle(bool isActive)
    {
        if (puzzleObject != null)
        {
            puzzleObject.SetActive(isActive);
        }
    }

    private void LoadNextScene()//���� ��Ʈ�ѷ����� action�� ���� ȣ���մϴ�.
    {
        ActivePuzzle(false);
        Destroy(gameObject);

        GameManager.Instance.OnPendantCollected(pendantColor, sceneName);
    }
}
