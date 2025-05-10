using UnityEngine;

public class UnloadScene : BaseTrigger
{

    public void UnloadMemoryScene()
    {
        CustomSceneManager.Instance.UnloadAdditiveScene();
    }

    protected override void OnPlayerEnter()
    {
        UnloadMemoryScene();
    }
}
