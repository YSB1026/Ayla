using UnityEngine;

public class UnloadScene : MonoBehaviour
{
    public void UnloadMemoryScene()
    {
        CustomSceneManager.Instance.UnloadMemoryScene();
    }
}
