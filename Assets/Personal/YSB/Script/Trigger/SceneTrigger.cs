using NUnit.Framework;
using UnityEngine;

public class SceneTrigger : BaseTrigger
{
    [SerializeField] private string sceneName;

    protected override void OnPlayerEnter()
    {
        CustomSceneManager.Instance.LoadScene(sceneName);
    }
}
