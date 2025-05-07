using System;
using UnityEngine;
using static LightColorController;

public class PendantEvent : MonoBehaviour
{
    //∆€¡Ò
    [Header("»πµÊ«“ ∫˚")]
    public ColorOption pendantColor;

    [Header("»∏ªÛ æ¿")]
    [SerializeField] private string sceneName;

    [Header("∆€¡Ò")]
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

    private void LoadNextScene()//∆€¡Ò ƒ¡∆Æ∑—∑Øø°º≠ action¿ª ≈Î«ÿ »£√‚«’¥œ¥Ÿ.
    {
        ActivePuzzle(false);
        Destroy(gameObject);

        GameManager.Instance.OnPendantCollected(pendantColor, sceneName);
    }
}
