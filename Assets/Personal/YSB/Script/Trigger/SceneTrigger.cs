using NUnit.Framework;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class SceneTrigger : BaseTrigger
{
    [SerializeField] private string sceneName;
    [SerializeField] private bool isAdditiveMode = false;
    [SerializeField] private bool isTrigger = false;

    protected override void OnPlayerEnter()
    {
        if (isTrigger) return;

        isTrigger = true;
        
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
