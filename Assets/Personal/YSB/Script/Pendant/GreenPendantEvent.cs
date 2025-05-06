using System;
using UnityEngine;

public class GreenPendantEvent : MonoBehaviour
{
    public static Action OnGreenPuzzleSolved;
    [SerializeField] private GameObject puzzleObject;
    [SerializeField] private GameObject sceneTrigger;

    private void Start()
    {
        if(!puzzleObject.activeSelf) puzzleObject.SetActive(false);
        if(!sceneTrigger.activeSelf) sceneTrigger.SetActive(false);
    }

    private void OnEnable()
    {
        OnGreenPuzzleSolved += LoadNextScene;
    }

    private void OnDisable()
    {
        OnGreenPuzzleSolved -= LoadNextScene;
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
    }

    private void LoadNextScene()
    {
        Debug.Log("·Îµå¾À");
        ActivePuzzle(false);
        Destroy(gameObject);

        sceneTrigger.SetActive(true);
        //GameManager.Instance.OnPendantCollected();
    }
}
