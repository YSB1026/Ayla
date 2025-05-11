using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObjectManager : MonoBehaviour
{
    [SerializeField] private List<ObjectAction> actions = new();


    public bool IsEmptyActions()
    {
        return actions.Count == 0;
    }

    public void ApplyActions()
    {
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
