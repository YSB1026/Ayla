using NUnit.Framework;
using UnityEngine;

public class SceneTrigger : BaseTrigger
{
    [SerializeField] private string sceneName;
    [SerializeField] private bool isAdditiveMode = false;

    protected override void OnPlayerEnter()
    {
        Debug.Log($"{gameObject.name} - Trigger");
        if (isAdditiveMode)
        {
            LoadAdditiveScene();
        }
        else
        {
            LoadScene();
        }
    }

    private void LoadScene()
    {
        CustomSceneManager.Instance.LoadScene(sceneName);
    }

    private void LoadAdditiveScene()
    {
        CustomSceneManager.Instance.LoadAdditiveScene(sceneName);
    }
}
