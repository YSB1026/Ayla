using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ObjectActionType
{
    None,
    SetActiveTrue,
    SetActiveFalse,
    Destroy
}

[System.Serializable]
public class ObjectAction
{
    public GameObject target;
    public ObjectActionType actionType;
}

public abstract class BaseTrigger : MonoBehaviour
{
    [Header("트리거 이후 오브젝트 활성화/비활성화, 파괴 설정")]
    [SerializeField] protected List<ObjectAction> actions = new();
    protected abstract void OnPlayerEnter();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPlayerEnter();
        }
    }

    public void ApplyActions()
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

}
