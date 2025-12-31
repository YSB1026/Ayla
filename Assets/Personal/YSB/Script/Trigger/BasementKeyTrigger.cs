using System.Collections.Generic;
using UnityEngine;

public class BasementKeyTrigger : MonoBehaviour
{
    [Header("트리거 이후 오브젝트 활성화/비활성화, 파괴 설정")]
    [SerializeField] private List<ObjectAction> actions = new();

    [Header("basement Lock")]
    [SerializeField] GameObject basementLock;

    [Header("추가로 삭제할 오브젝트들")]
    [SerializeField] public List<GameObject> objectsToDestroy = new();

    [Header("트리거 후 자기자신 삭제")]
    [SerializeField] bool destroySelfAfterTrigger = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == basementLock)
        {
            basementLock.GetComponent<Animator>().SetBool("isOpen", true);
            
            // actions 실행
            ApplyActions();
            
            // 추가 오브젝트들 삭제
            DestroyAdditionalObjects();
            
            if (destroySelfAfterTrigger)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ApplyActions()
    {
        if (actions.Count == 0) return;

        foreach (var action in actions)
        {
            if (action.target == null) continue;

            switch (action.actionType)
            {
                case ObjectActionType.SetActiveTrue:
                    action.target.SetActive(true);
                    break;
                case ObjectActionType.SetActiveFalse:
                    action.target.SetActive(false);
                    break;
                case ObjectActionType.Destroy:
                    Destroy(action.target);
                    break;
                default:
                    break;
            }
        }
    }

    private void DestroyAdditionalObjects()
    {
        if (objectsToDestroy.Count == 0) return;

        foreach (var obj in objectsToDestroy)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }
}