using System;
using UnityEngine;
using static LightColorController;

public class PendantEvent : MonoBehaviour
{
    //퍼즐
    [Header("획득할 빛")]
    public ColorOption pendantColor;

    [Header("회상 씬")]
    [SerializeField] private string sceneName;

    [Header("퍼즐")]
    [SerializeField] private GameObject puzzleObject;

    [Header("회상씬 이후 이벤트 트리거")]
    [SerializeField] private GameObject eventAfterRecallScene;

    public static Action OnPuzzleSolved;

    private void OnValidate()
    {
        if(puzzleObject == null)
        {
            Debug.LogError($"{gameObject.name} 퍼즐 넣어주세요!!");
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

    private void LoadNextScene()//퍼즐 컨트롤러에서 action을 통해 호출하는 함수에요.
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
